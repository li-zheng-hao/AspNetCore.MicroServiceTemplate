using MongoDB.Entities;

namespace MST.Infra.FileProvider.MongoDB;

public class NormalFile:FileEntity
{
    public string MD5 { get; set; }
    
}