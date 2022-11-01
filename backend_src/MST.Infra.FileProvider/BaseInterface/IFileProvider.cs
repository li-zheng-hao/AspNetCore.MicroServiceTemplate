namespace MST.Infra.FileProvider.BaseInterface;
/// <summary>
/// TODO 未确定各种存储方案的对象获取方式
/// </summary>
public interface IFileProvider
{
    public string GetFileUrl(FilesStorageOptions options,string objectId);
    public Task<MemoryStream> DownloadAsMemoryStreamAsync(FilesStorageOptions options,string objectId);
    public Task<byte[]> DownloadAsBytesAsync(FilesStorageOptions options,string objectId);
}