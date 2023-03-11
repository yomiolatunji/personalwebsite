using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YomiOlatunji.DataSource.Services.Interfaces
{
    public interface IFileManager
    {
        Task<string> SaveFile(byte[] bytes, string fileName, string subFolder = "", string? suffix = null);
    }
}
