using Contract.Dtos.Requests;
using Repository.Models;
using AutoMapperProfile = AutoMapper.Profile;
namespace InnerChildApi.Common.AutoMapper
{
    public class MappingProfile : AutoMapperProfile
    {
        public  MappingProfile()
        {
            //CreateMap<UpdateArticleRequest, Article>()
            //    .ForMember(dest => dest.ArticleId, opt => opt.MapFrom(src => src.ArticleId))
            //    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
               
        }
    }
}
