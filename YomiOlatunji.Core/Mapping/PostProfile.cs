using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using YomiOlatunji.Core.DbModel.Post;
using YomiOlatunji.Core.ViewModel;

namespace YomiOlatunji.Core.Mapping
{
    public class PostProfile : Profile
    {
        public PostProfile()
        {
            CreateMap<CreatePost, Post>()
                .ForMember(dest => dest.IsPublished,
                    opt => opt.MapFrom(a => false))
                .ForMember(dest => dest.CanComment,
                    opt => opt.MapFrom(src => true))
                .ForMember(dest=>dest.IsArchived,
                    opt => opt.MapFrom(a => false))
                .ReverseMap();

            CreateMap<AddContactMessage, ContactMessage>()
                .ReverseMap();
        }
    }
}
