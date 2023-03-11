using AutoMapper;
using YomiOlatunji.Core;
using YomiOlatunji.Core.DbModel.Post;
using YomiOlatunji.Core.ViewModel;
using YomiOlatunji.DataSource.Interface;
using YomiOlatunji.DataSource.Services.Interfaces;

namespace YomiOlatunji.DataSource.Services
{
    public class ContactMessageService : IContactMessageService
    {
        private readonly IContactMessageRepository _repository;
        private readonly IMapper mapper;

        public ContactMessageService(IContactMessageRepository repository, IMapper mapper)
        {
            _repository = repository;
            this.mapper = mapper;
        }
        public async Task<bool> AddMessage(AddContactMessage message)
        {
            var addMessage = mapper.Map<ContactMessage>(message);
            //TODO: send notification as mail
            _repository.Add(addMessage);
            return await _repository.SaveChanges();
        }

        public async Task<IList<ContactMessage>> GetAllContactMessages()
        {
            return (await _repository.Get()).ToList();
        }

        public async Task<PageModel<ContactMessage>> GetContactMessages(int intPageIndex = 1, int intPageSize = 20)
        {
            return await _repository.GetPage(null, intPageIndex, intPageSize);

        }
    }
}
