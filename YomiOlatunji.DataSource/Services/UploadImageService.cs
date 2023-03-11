using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YomiOlatunji.Core.ViewModel;
using YomiOlatunji.DataSource.Extensions;
using YomiOlatunji.DataSource.Services.Interfaces;

namespace YomiOlatunji.DataSource.Services
{
    public class UploadImageService : IUploadImageService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UploadImageService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public string Test()
        {
            return "Api is working";
        }

        public UploadFileResult Upload(IFormFile file)
        {
            if (file.Length == 0)
                return null;

            UploadFileResult result = new UploadFileResult();
            var filename= GetUniqueFileName(file.FileName);
            var path = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot\\Upload", filename);
            var dir = Path.GetDirectoryName(path);
            if (dir != null && !Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            var baseUrl = _httpContextAccessor.HttpContext?.Request.BaseUrl();
            var remotePath = Path.Combine(
                baseUrl, "Upload", filename);
            result.Location = remotePath;
            result.FileName = filename;
            return result;
        }
        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                      + "_"
                      + Guid.NewGuid().ToString().Substring(0, 4)
                      + Path.GetExtension(fileName);
        }
    }
}
