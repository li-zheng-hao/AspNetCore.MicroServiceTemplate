using System.Diagnostics;
using System.Reflection;
using AspNetCore.StartUpTemplate.Filter;
using FreeRedis;
using FreeSql;
using FreeSql.Internal;
using Mapster;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MST.Infra.Configuration;
using MST.Infra.Shared.Contract.DTO;
using MST.Infra.Shared.Filter;
using MST.Infra.Shared.HttpHandler;
using Nacos.AspNetCore.V2;
using Nacos.V2.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Polly;
using Refit;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;

namespace MST.Infra.Shared;

/// <summary>
/// 通用服务注入
/// </summary>
public static class ServiceDependencyInjection
{
   
    #region Nacos

    public static IServiceCollection AddNacosConfigurationCenter(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddNacosV2Config(configuration, sectionName: "NacosConfig");
        return services;
    }

    public static IServiceCollection AddNacosServiceDiscoveryCenter(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddNacosAspNet(configuration, section: "Nacos");
        return services;
    }

  

    #endregion

    #region FreeSql

    /// <summary>
    /// FreeSql
    /// </summary>
    /// <param name="services"></param>
    /// <param name="c"></param>
    public static IServiceCollection AddFreeSql(this IServiceCollection services, IConfiguration configuration,
        Assembly? repoAssembly)
    {
        var mysqlOptions = configuration.GetSection("Mysql").Get<MysqlOptions>();
        Func<IServiceProvider, IFreeSql> fsql = r =>
        {
            IFreeSql fsql = new FreeSqlBuilder()
                .UseConnectionString(DataType.MySql, mysqlOptions.ConnectionString)
                .UseNameConvert(NameConvertType.PascalCaseToUnderscoreWithLower)
#if DEBUG
                .UseAutoSyncStructure(true)
#else
                    .UseAutoSyncStructure(false)
#endif

                .UseNoneCommandParameter(true)
                .UseMonitorCommand(cmd => { Trace.WriteLine($"freesql监视命令 {cmd.CommandText}"); }
                )
                .Build();
            fsql.Aop.CurdAfter += (s, e) =>
            {
                if (e.ElapsedMilliseconds > 200)
                {
                    //记录日志
                    //发送短信给负责人
                    Log.Logger.Warning($"慢sql 耗时{e.ElapsedMilliseconds}毫秒 语句{e.Sql}");
                }
            };

            // fsql.UseJsonMap();
            return fsql;
        };

        services.AddSingleton(fsql);
        services.AddScoped<UnitOfWorkManager>();
        if(repoAssembly is not null)
            services.AddFreeRepository(null,repoAssembly);
        return services;
    }

    #endregion
    #region DTM

    /// <summary>
    /// 配置DTM
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddDtm(this IServiceCollection services)
    {
        // services.AddDtmcli(dtm =>
        // {
        //     
        //     dtm.DtmUrl = GlobalConfig.Instance.Dtm.DtmUrl;
        //     dtm.BarrierTableName = "barrier";
        // });
        return services;
    }

    #endregion

    #region Json配置

    public static IMvcBuilder AddCustomJson(this IMvcBuilder mvcBuilder)
    {
        mvcBuilder.AddNewtonsoftJson(p =>
        {
            //数据格式首字母小写 不使用驼峰
            p.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            //不使用驼峰样式的key
            //p.SerializerSettings.ContractResolver = new DefaultContractResolver();
            p.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            p.SerializerSettings.DateFormatString = "yyyy/MM/dd HH:mm:ss";
        });
        JsonConvert.DefaultSettings = () => new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            DateFormatString = "yyyy/MM/dd HH:mm:ss",
        };
        return mvcBuilder;
    }

    #endregion

    #region refitclient 微服务间调用

    public static IServiceCollection AddCustomRefitClient<TRestClient>(this IServiceCollection collection,
        List<IAsyncPolicy<HttpResponseMessage>> policies, string serviceName) where TRestClient : class
    {
        collection.TryAddScoped<TokenDelegatingHandler>();
        collection.TryAddScoped<NacosDiscoverDelegatingHandler>();
        var refitSettings = new RefitSettings();
        refitSettings.ContentSerializer = new NewtonsoftJsonContentSerializer();
        var clientBuilder = collection.AddRefitClient<TRestClient>(refitSettings)
            .SetHandlerLifetime(TimeSpan.FromMinutes(2))
            .AddPolicyHandlerICollection(policies)
            // 配置Token
            .AddHttpMessageHandler<TokenDelegatingHandler>()
            // 配置Handler
            .AddHttpMessageHandler<NacosDiscoverDelegatingHandler>();
        clientBuilder.ConfigureHttpClient(client => client.BaseAddress = new Uri($"http://{serviceName}"));
        return collection;
    }

    #endregion

    #region 日志配置

    public static IServiceCollection AddCustomSerilog(this IServiceCollection collection, IHostBuilder hostBuilder,
        IConfiguration envConfiguration, IWebHostEnvironment environment)
    {
        var section = envConfiguration.GetSection(typeof(ElasticSearchOptions).GetDescription());
        var elasticSearchOptions = section.Get<ElasticSearchOptions>();

        var commonOptions = envConfiguration.GetSection(typeof(CommonOptions).GetDescription()).Get<CommonOptions>();

        if (section is null)
        {
            throw new Exception("Elasticsearch配置不存在!");
        }

        hostBuilder.UseSerilog((context, services, configuration) =>
        {
            const string OUTPUT_TEMPLATE =
                "[{Level}] [{TraceId}] {ENV} {Timestamp:yyyy-MM-dd HH:mm:ss.fff} {SourceContext} {Message:lj}{NewLine}{Exception}";
            configuration
#if DEBUG
                .MinimumLevel.Debug()
#else
            .MinimumLevel.Information()
#endif
                .Enrich.WithProperty("ENV", environment.EnvironmentName)
                .Enrich.WithMachineName()
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .WriteTo.Console(outputTemplate: OUTPUT_TEMPLATE)
                .WriteTo.Skywalking(services)
                .WriteTo.File("logs/applog_.log"
                    , rollingInterval: RollingInterval.Day
                    , outputTemplate: OUTPUT_TEMPLATE);
            // 如果有elasticsearch则写入
            if (elasticSearchOptions?.Url?.IsNotNullOrWhiteSpace() == true)
                configuration.WriteTo.Elasticsearch(
                    new ElasticsearchSinkOptions(
                            new Uri(elasticSearchOptions.Url)) // for the docker-compose implementation
                        {
                            AutoRegisterTemplate = true,
                            // OverwriteTemplate = true,
                            DetectElasticsearchVersion = true,
                            AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
                            // NumberOfReplicas = 1,
                            // NumberOfShards = 2,
                            // BufferBaseFilename = "logs/buffer",
                            // RegisterTemplateFailure = RegisterTemplateRecovery.FailSink,
                            // FailureCallback = e => Console.WriteLine("Unable to submit event " + e.MessageTemplate),
                            // EmitEventFailure = EmitEventFailureHandling.WriteToSelfLog |
                            // EmitEventFailureHandling.WriteToFailureSink |
                            // EmitEventFailureHandling.RaiseCallback,
                            // FailureSink = new FileSink("logs/fail-{Date}.txt", new JsonFormatter(), null, null)
                        });
        
        });
        return collection;
    }

    #endregion

    #region CAP配置

    public static IServiceCollection AddCustomCAP(this IServiceCollection collection, IConfiguration configuration)
    {
        var mysqlOptions = configuration.GetSection(typeof(MysqlOptions).GetDescription()).Get<MysqlOptions>();
        var capOptions = configuration.GetSection(typeof(CapOptions).GetDescription()).Get<CapOptions>();
        var rabbitMQOptions = configuration.GetSection(typeof(RabbitMQOptions).GetDescription()).Get<RabbitMQOptions>();
        collection.AddCap(x =>
        {
            x.UseDashboard();
            x.UseMySql(mysqlOptions.ConnectionString);
            x.UseRabbitMQ(it =>
            {
                it.HostName = rabbitMQOptions.Host;
                it.Port = rabbitMQOptions.Port;
                it.UserName = rabbitMQOptions.UserName;
                it.Password = rabbitMQOptions.Password;
                it.VirtualHost = rabbitMQOptions.VirtualHost;
            });
            x.FailedRetryCount = 5;
            x.FailedThresholdCallback = failed =>
            {
                var notifyStr = string.Format("消息重试达到上限次数,消息主题 {0},消息重试次数{1}",
                    JsonConvert.SerializeObject(failed.Message), x.FailedRetryCount);
                Log.Logger.Error(notifyStr);
                // todo 短信/邮件通知异常
            };
        });
        return collection;
    }

    #endregion

    #region Redis及缓存配置

    public static IServiceCollection AddFreeRedis(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        var redisOptions = configuration.GetSection(typeof(RedisOptions).GetDescription()).Get<RedisOptions>();
        // 单机
        RedisClient redisClient = new RedisClient(
            redisOptions.ConnectionString
        );
        // 哨兵集群配置
        // RedisClient redisClient = new RedisClient(
        //     redisOptions.ConnectionString,
        //     redisOptions.SentinelAdders,
        //     true //是否读写分离
        // );
        serviceCollection.AddSingleton<IRedisClient>(redisClient);
        return serviceCollection;
    }

    /// <summary>
    /// 配置CacheOutput库用于接口缓存
    /// </summary>
    /// <param name="collection"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    // public static IServiceCollection AddCustomRedisCacheOutput(
    //     this IServiceCollection collection)
    // {
    //     if (services == null)
    //         throw new ArgumentNullException(nameof(services));
    //     services.TryAdd(ServiceDescriptor.Singleton<CacheKeyGeneratorFactory, CacheKeyGeneratorFactory>());
    //     services.TryAdd(ServiceDescriptor.Singleton<ICacheKeyGenerator, DefaultCacheKeyGenerator>());
    //     services.TryAdd(ServiceDescriptor.Singleton<IApiCacheOutput, StackExchangeRedisCacheOutputProvider>());
    //     ConfigurationOptions options = new ConfigurationOptions();
    //     options.Password = GlobalConfig.Instance.Redis.Password;
    //     options.CommandMap = CommandMap.Sentinel;
    //     foreach (var addr in GlobalConfig.Instance.Redis.SentinelAdders)
    //     {
    //         var ipPort = addr.Split(':');
    //         options.EndPoints.Add(ipPort[0], Convert.ToInt32(ipPort[1]));
    //     }
    //     options.TieBreaker = ""; //这行在sentinel模式必须加上
    //     options.DefaultVersion = new Version(3, 0);
    //     options.AllowAdmin = true;
    //     var masterConfig = new ConfigurationOptions
    //     {
    //         CommandMap = CommandMap.Default,
    //         ServiceName = GlobalConfig.Instance.Redis.ServiceName,
    //         Password = GlobalConfig.Instance.Redis.Password,
    //         AllowAdmin = true
    //     };
    //     services.TryAdd(
    //         ServiceDescriptor.Singleton<IConnectionMultiplexer>(
    //             (IConnectionMultiplexer)ConnectionMultiplexer.Connect(options)));
    //     services.TryAdd(ServiceDescriptor.Transient<IDatabase>((Func<IServiceProvider, IDatabase>)(e =>
    //         ((ConnectionMultiplexer)e.GetRequiredService<IConnectionMultiplexer>())
    //         .GetSentinelMasterConnection(masterConfig).GetDatabase())));
    //     return services;
    // }

    #endregion

    #region 跨域

    public static IServiceCollection AddCustomCors(this IServiceCollection serviceCollection)
    {
        // 此处根据自己的需要配置可通过的域名或ip
        serviceCollection.AddCors(options =>
        {
            options.AddDefaultPolicy(
                policy =>
                {
                    policy.SetIsOriginAllowed(it => true);
                    policy.AllowAnyHeader();
                    policy.AllowAnyMethod();
                    policy.AllowCredentials();
                });
        });
        return serviceCollection;
    }

    #endregion

    #region Mapster映射

    public static IServiceCollection AddMapster(this IServiceCollection serviceCollection)
    {
        var typeAdapterConfig = TypeAdapterConfig.GlobalSettings;
        // 全局忽略大小写
        TypeAdapterConfig.GlobalSettings.Default.NameMatchingStrategy(NameMatchingStrategy.IgnoreCase);
        Assembly applicationAssembly = typeof(BaseDto<,>).Assembly;
        typeAdapterConfig.Scan(applicationAssembly);
        return serviceCollection;
    }

    #endregion

    #region Swagger配置

    public static IServiceCollection AddCustomSwaggerGen(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSwaggerGen(c =>
        {
            //添加Authorization
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme.",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Scheme = "bearer",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT"
            });
            // 接口文档抓取
            var xmlFile = $"{Assembly.GetEntryAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            //... and tell Swagger to use those XML comments.
            if (File.Exists(xmlPath))
            {
                c.IncludeXmlComments(xmlPath, true);
            }
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                    },
                    new List<string>()
                }
            });
            //允许上传文件
            c.OperationFilter<FileUploadFilter>();
            c.DocumentFilter<SwaggerIgnoreFilter>();
        });
        return serviceCollection;
    }

    #endregion

    #region 过滤器

    public static IMvcBuilder AddCustomMvc(this IServiceCollection collection)
    {
        return collection.AddMvc(options =>
        {
            // 自定义模型验证
            // options.Filters.Add<ModelValidatorFilter>();
            //异常处理
            options.Filters.Add<GlobalExceptionsFilter>();
        });
    }

    #endregion
}