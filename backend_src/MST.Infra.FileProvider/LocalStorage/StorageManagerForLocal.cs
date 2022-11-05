using System.Drawing;
using System.Globalization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Quickwire.Attributes;

namespace MST.Infra.FileProvider.LocalStorage;

[RegisterService(ServiceLifetime.Singleton,ServiceType = typeof(IStorageManagerForLocal))]
class StorageManagerForLocal : IStorageManagerForLocal
{
    private readonly string _contentRootPath;

    public StorageManagerForLocal(IHostingEnvironment hostingEnvironment)
    {
        _contentRootPath=hostingEnvironment.ContentRootPath;
    }
    /// <summary>
    /// 本地上传方法
    /// </summary>
    /// <param name="options"></param>
    /// <param name="memStream"></param>
    /// <param name="filesStorageLocation"></param>
    /// <returns></returns>
    public string UpLoad(FilesStorageOptions options, MemoryStream memStream)
    {

        var newFileName = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo) + ".jpg";
        var today = DateTime.Now.ToString("yyyyMMdd");

        Image mImage = Image.FromStream(memStream);
        Bitmap bp = new Bitmap(mImage);

        var saveUrl = Path.Combine( options.Path ,today , "/");
        var dirPath =  Path.Combine(_contentRootPath, saveUrl);

        string bucketBindDomain = options.ApiDomain;

        if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);
        var filePath = dirPath + newFileName;
        var fileUrl = saveUrl + newFileName;

        bp.Save(filePath, System.Drawing.Imaging.ImageFormat.Jpeg);//注意保存路径

        return bucketBindDomain + fileUrl;
    }
}