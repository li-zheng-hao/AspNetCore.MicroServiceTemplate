using System.Globalization;
using Aliyun.OSS;
using Aliyun.OSS.Util;
using Microsoft.AspNetCore.Http;

namespace MST.Infra.FileProvider.Aliyun;

public interface IStorageManagerForAliYunCOS
{
    Task<string> UpLoad(FilesStorageOptions options, IFormFile file);
    string UpLoad(FilesStorageOptions options, MemoryStream fileStream, string fileName);
    string UpLoad(FilesStorageOptions options, byte[] bytes, string fileName);
}
