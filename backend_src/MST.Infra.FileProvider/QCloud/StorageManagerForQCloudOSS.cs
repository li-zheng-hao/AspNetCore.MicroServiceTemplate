using System.Globalization;
using COSXML;
using COSXML.Auth;
using Microsoft.AspNetCore.Http;

namespace MST.Infra.FileProvider.QCloud;

class StorageManagerForQCloudOSS : IStorageManagerForQCloudOSS
{
    public async Task<string> UpLoad(FilesStorageOptions options, string fileExt, IFormFile file)
    {

        var newFileName = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo) + fileExt;
        var today = DateTime.Now.ToString("yyyyMMdd");

        var filePath = options.Path + today + "/" + newFileName; //云文件保存路径

        //上传到腾讯云OSS
        //初始化 CosXmlConfig
        string appid = options.AccountId;//设置腾讯云账户的账户标识 APPID
        string region = options.CosRegion; //设置一个默认的存储桶地域
        CosXmlConfig config = new CosXmlConfig.Builder()
            //.SetAppid(appid)
            .IsHttps(true)  //设置默认 HTTPS 请求
            .SetRegion(region)  //设置一个默认的存储桶地域
            .SetDebugLog(true)  //显示日志
            .Build();  //创建 CosXmlConfig 对象

        long durationSecond = 600;  //每次请求签名有效时长，单位为秒
        QCloudCredentialProvider qCloudCredentialProvider = new DefaultQCloudCredentialProvider(options.AccessKeyId, options.AccessKeySecret, durationSecond);


        byte[] bytes;
        await using (var ms = new MemoryStream())
        {
            await file.CopyToAsync(ms);
            bytes = ms.ToArray();
        }

        var cosXml = new CosXmlServer(config, qCloudCredentialProvider);
        COSXML.Model.Object.PutObjectRequest putObjectRequest = new COSXML.Model.Object.PutObjectRequest(options.TencentBucketName, filePath, bytes);
        cosXml.PutObject(putObjectRequest);

        return options.BucketBindUrl + filePath;
    }

    public string Upload(FilesStorageOptions options, byte[] bytes)
    {
        var newFileName = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo) + ".jpg";
        var today = DateTime.Now.ToString("yyyyMMdd");

        //初始化 CosXmlConfig
        string appid = options.AccountId;//设置腾讯云账户的账户标识 APPID
        string region = options.CosRegion; //设置一个默认的存储桶地域
        CosXmlConfig config = new CosXmlConfig.Builder()
            //.SetAppid(appid)
            .IsHttps(true)  //设置默认 HTTPS 请求
            .SetRegion(region)  //设置一个默认的存储桶地域
            .SetDebugLog(true)  //显示日志
            .Build();  //创建 CosXmlConfig 对象
        long durationSecond = 600;  //每次请求签名有效时长，单位为秒
        QCloudCredentialProvider qCloudCredentialProvider = new DefaultQCloudCredentialProvider(options.AccessKeyId, options.AccessKeySecret, durationSecond);
        var cosXml = new CosXmlServer(config, qCloudCredentialProvider);
        var filePath = options.Path + today + "/" + newFileName; //云文件保存路径
        COSXML.Model.Object.PutObjectRequest putObjectRequest = new COSXML.Model.Object.PutObjectRequest(options.TencentBucketName, filePath, bytes);

        cosXml.PutObject(putObjectRequest);

        return options.BucketBindUrl + filePath;
    }
}