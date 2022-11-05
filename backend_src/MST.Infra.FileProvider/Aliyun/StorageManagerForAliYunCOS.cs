using System.Globalization;
using Aliyun.OSS;
using Aliyun.OSS.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Quickwire.Attributes;

namespace MST.Infra.FileProvider.Aliyun;

[RegisterService(ServiceLifetime.Singleton,ServiceType = typeof(IStorageManagerForAliYunCOS))]
public class StorageManagerForAliYunCOS : IStorageManagerForAliYunCOS
{

    public async Task<string> UpLoad(FilesStorageOptions options, IFormFile file)
    {
        var newFileName = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo) + Path.GetExtension(file.FileName);
        var today = DateTime.Now.ToString("yyyyMMdd");

        await using var fileStream = file.OpenReadStream();
        var md5 = OssUtils.ComputeContentMd5(fileStream, file.Length);

        var filePath = options.Path + today + "/" + newFileName; 
        var aliYun = new OssClient(options.Endpoint, options.AccessKeyId, options.AccessKeySecret);
        var objectMeta = new ObjectMetadata
        {
            ContentMd5 = md5
        };
        aliYun.PutObject(options.BucketName, filePath, fileStream, objectMeta);
        return options.BucketBindUrl + filePath;
    }

    public string UpLoad(FilesStorageOptions options, MemoryStream fileStream,string fileName)
    {
        var newFileName = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo) + Path.GetExtension(fileName);
        var today = DateTime.Now.ToString("yyyyMMdd");
        var md5 = OssUtils.ComputeContentMd5(fileStream, fileStream.Length);

        var filePath = options.Path + today + "/" + newFileName; 
        var aliYun = new OssClient(options.Endpoint, options.AccessKeyId, options.AccessKeySecret);
        var objectMeta = new ObjectMetadata
        {
            ContentMd5 = md5
        };
        aliYun.PutObject(options.BucketName, filePath, fileStream, objectMeta);
        return options.BucketBindUrl + filePath;
    }

    public string UpLoad(FilesStorageOptions options, byte[] bytes,string fileName)
    {
        var newFileName = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo) + Path.GetExtension(fileName);
        var today = DateTime.Now.ToString("yyyyMMdd");
        MemoryStream fileStream = new MemoryStream(bytes);
        var md5 = OssUtils.ComputeContentMd5(fileStream, fileStream.Length);

        var filePath = options.Path + today + "/" + newFileName; //云文件保存路径
        var aliYun = new OssClient(options.Endpoint, options.AccessKeyId, options.AccessKeySecret);
        var objectMeta = new ObjectMetadata
        {
            ContentMd5 = md5
        };
        aliYun.PutObject(options.BucketName, filePath, fileStream, objectMeta);
        return options.BucketBindUrl + filePath;
    }
}