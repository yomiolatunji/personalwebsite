using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using YomiOlatunji.DataSource.Repository;
using YomiOlatunji.DataSource.Services.Interfaces;

namespace YomiOlatunji.DataSource.Services
{
    public class FileManager: IFileManager
    {
        private string CleanFromInvalidChars(string input)
        {
            var regexSearch = Regex.Escape(new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars()));
            var r = new Regex($"[{regexSearch}]");
            return r.Replace(input, string.Empty);
        }
        public async Task<string> SaveFile(byte[] bytes, string fileName, string subFolder = "", string suffix = null)
        {
            if (bytes is null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }

            suffix = CleanFromInvalidChars(!string.IsNullOrWhiteSpace(suffix) ? suffix : DateTime.UtcNow.Ticks.ToString(CultureInfo.InvariantCulture));

            string folderName = "wwwroot";// Path.Combine("Resources", "Images");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

            var ext = Path.GetExtension(fileName);
            var name = CleanFromInvalidChars(Path.GetFileNameWithoutExtension(fileName));

            var fileNameWithSuffix = $"{name}_{suffix}{ext}";
            
            var fullPath = Path.Combine(pathToSave, fileNameWithSuffix);
            var dbPath = Path.Combine(folderName, fileNameWithSuffix);
            try
            {

                //var dir = Path.GetDirectoryName(fullPath);

                ///Directory.CreateDirectory(dir);
                using (var writer = new FileStream(fullPath, FileMode.CreateNew))
                {
                    await writer.WriteAsync(bytes, 0, bytes.Length).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                //_ = logRepository.AddErrorLogFromException(ex);
                throw ex;
            }

            return dbPath;// Path.Combine(subFolder, "files", fileNameWithSuffix);// $"/{POST}/files/{fileNameWithSuffix}";
        }

    }
}
