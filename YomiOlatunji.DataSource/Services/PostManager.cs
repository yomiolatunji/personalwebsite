using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using YomiOlatunji.Core.DbModel.Post;
using YomiOlatunji.DataSource.Services.Interfaces;

namespace YomiOlatunji.DataSource.Services
{
    public class PostManager : IPostManager
    {
        private readonly IFileManager _fileManager;

        public PostManager(IFileManager fileManager)
        {
            _fileManager = fileManager;
        }
        public async Task<string> ExtractImageFromPost(string content)
        {
            var imgRegex = new Regex("<img[^>]+ />", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            var base64Regex = new Regex("data:[^/]+/(?<ext>[a-z]+);base64,(?<base64>.+)", RegexOptions.IgnoreCase);
            var allowedExtensions = new[] {
              ".jpg",
              ".jpeg",
              ".gif",
              ".png",
              ".webp"
            };

            foreach (Match match in imgRegex.Matches(content))
            {
                if (match is null)
                {
                    continue;
                }

                var doc = new XmlDocument();
                doc.LoadXml($"<root>{match.Value}</root>");

                var img = doc.FirstChild?.FirstChild;
                var srcNode = img?.Attributes?["src"];
                var fileNameNode = img?.Attributes?["data-filename"];

                // The HTML editor creates base64 DataURIs which we'll have to convert to image
                // files on disk
                if (srcNode is null || fileNameNode is null)
                {
                    continue;
                }

                var extension = Path.GetExtension(fileNameNode.Value);

                // Only accept image files
                if (!allowedExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase))
                {
                    continue;
                }

                var base64Match = base64Regex.Match(srcNode.Value);
                if (base64Match.Success)
                {
                    var bytes = Convert.FromBase64String(base64Match.Groups["base64"].Value);
                    srcNode.Value = await _fileManager.SaveFile(bytes, fileNameNode.Value).ConfigureAwait(false);

                    img?.Attributes?.Remove(fileNameNode);
                    content = content.Replace(match.Value, img?.OuterXml, StringComparison.OrdinalIgnoreCase);
                }
            }
            return content;
        }

        public string ExtractUrlFromTitle(string value)
        {
            value = value.ToLowerInvariant();

            //Remove all accents
            var bytes = Encoding.GetEncoding("Cyrillic").GetBytes(value);

            value = Encoding.ASCII.GetString(bytes);

            //Replace spaces 
            value = Regex.Replace(value, @"\s", "-", RegexOptions.Compiled);

            //Remove invalid chars 
            value = Regex.Replace(value, @"[^\w\s\p{Pd}]", "", RegexOptions.Compiled);

            //Trim dashes from end 
            value = value.Trim('-', '_');

            //Replace double occurences of - or \_ 
            value = Regex.Replace(value, @"([-_]){2,}", "$1", RegexOptions.Compiled);

            return value;
        }

        public string ExtractExcerptFromPost(string content)
        {
            var text = StripHtml(content);
            var s= text.Substring(0, Math.Min(200, text.Length));
            s = HttpUtility.HtmlDecode(s);
            return s;
        }

        private string CleanFromInvalidChars(string input)
        {
            var allowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-_~";
            var regexSearch = Regex.Escape(allowedChars);
            var r = new Regex($"[^{regexSearch}]");
            //var r = new Regex($"[^a-zA-Z\d\s:]");
            return r.Replace(input, string.Empty);
        }
        public string StripHtml(string htmlText)
        {
            Regex reg = new Regex("<[^>]+>", RegexOptions.IgnoreCase);
            return reg.Replace(htmlText, "");
        }
    }
}
