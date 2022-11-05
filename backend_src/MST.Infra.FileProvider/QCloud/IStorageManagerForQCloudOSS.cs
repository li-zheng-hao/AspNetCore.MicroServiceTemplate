using Microsoft.AspNetCore.Http;

namespace MST.Infra.FileProvider.QCloud;

public interface IStorageManagerForQCloudOSS
{
    /// <summary>
    /// 腾讯云存储上传方法（File）
    /// </summary>
    /// <param name="options"></param>
    /// <param name="fileExt"></param>
    /// <param name="file"></param>
    /// <returns></returns>
    Task<string> UpLoad(FilesStorageOptions options, string fileExt, IFormFile file);

    /// <summary>
    /// 腾讯云存储上传方法（Base64）
    /// </summary>
    /// <param name="options"></param>
    /// <param name="bytes"></param>
    /// <returns></returns>
    string Upload(FilesStorageOptions options, byte[] bytes);
}
