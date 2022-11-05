
using Microsoft.AspNetCore.Http;

namespace MST.Infra.FileProvider.MongoDB;

public interface IStorageManagerForMongo
{
    Task<string> Upload(IFormFile file);
}


