using YomiOlatunji.Core.DbModel.Post;
using YomiOlatunji.DataSource.Interface;

namespace YomiOlatunji.DataSource.Repository
{
    public class ContactMessageRepository : BaseRepository<ContactMessage>, IContactMessageRepository
    {
        public ContactMessageRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
