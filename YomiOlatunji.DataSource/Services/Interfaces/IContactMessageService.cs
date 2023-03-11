using YomiOlatunji.Core;
using YomiOlatunji.Core.DbModel.Post;
using YomiOlatunji.Core.ViewModel;

namespace YomiOlatunji.DataSource.Services.Interfaces
{
    public interface IContactMessageService
    {
        Task<PageModel<ContactMessage>> GetContactMessages(int intPageIndex = 1, int intPageSize = 20);
        Task<IList<ContactMessage>> GetAllContactMessages();
        Task<bool> AddMessage(AddContactMessage message);
    }
}
