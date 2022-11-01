using Microsoft.AspNetCore.Http;

namespace MST.Infra.FileProvider.BaseInterface;

public interface IFileUploader
{
    public Task<string> UpLoadAsync(FilesStorageOptions options, IFormFile file);
    public Task<string> UpLoadAsync(FilesStorageOptions options, MemoryStream memStream);
    public Task<string> UpLoadAsync(FilesStorageOptions options, byte[] bytes);
}