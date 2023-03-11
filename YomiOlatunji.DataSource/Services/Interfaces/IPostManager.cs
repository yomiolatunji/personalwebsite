using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YomiOlatunji.DataSource.Services.Interfaces
{
    public interface IPostManager
    {
        Task<string> ExtractImageFromPost(string content);
        string ExtractUrlFromTitle(string title);
        string ExtractExcerptFromPost(string content);
    }
}
