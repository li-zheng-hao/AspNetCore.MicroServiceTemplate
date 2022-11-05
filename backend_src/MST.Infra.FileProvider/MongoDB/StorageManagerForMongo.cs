using System.Security.Cryptography;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Entities;
using MST.Infra.Utility.Helper;
using Quickwire.Attributes;

namespace MST.Infra.FileProvider.MongoDB;

[RegisterService(ServiceLifetime.Singleton,ServiceType = typeof(IStorageManagerForMongo))]
public class StorageManagerForMongo:IStorageManagerForMongo
{
    public async Task<string> Upload(IFormFile file)
    {
        var mongoFile= new NormalFile();
        var stream=file.OpenReadStream();
        mongoFile.MD5=EncryptHelper.Encrypt(stream);
        await mongoFile.SaveAsync();
        stream.Seek(0,SeekOrigin.Begin);
        await mongoFile.Data.UploadAsync(stream);
        return mongoFile.ID;
    }

    public async Task<Stream> Download(string objectId)
    {
        var memoryStream = new MemoryStream();
        await DB.File<NormalFile>(objectId).DownloadWithTimeoutAsync(memoryStream, 10);
        return memoryStream;
    }
}