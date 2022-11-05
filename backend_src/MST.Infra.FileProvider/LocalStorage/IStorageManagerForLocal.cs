using System.Drawing;
using System.Globalization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Quickwire.Attributes;

namespace MST.Infra.FileProvider.LocalStorage;

public interface IStorageManagerForLocal
{ 
    string UpLoad(FilesStorageOptions options, MemoryStream memStream);
}
