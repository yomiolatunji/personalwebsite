using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YomiOlatunji.Core.ViewModel;

namespace YomiOlatunji.DataSource.Services.Interfaces
{
    public interface IUploadImageService
    {
        string Test();
        UploadFileResult Upload(IFormFile file);
    }
}
