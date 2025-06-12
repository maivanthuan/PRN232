using AutoMapper;
using DataAccessObjects.DTO;
using BusinessObjects.Models;

namespace FUNewsManagementSystem.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<SystemAccount, AccountDto>();
            CreateMap<AccountCreateDto, SystemAccount>();
			CreateMap<CategoryDto, Category>()
				.ForMember(dest => dest.CategoryId, opt => opt.Ignore());
			CreateMap<CategoryDto, Category>().ReverseMap();
			CreateMap<Tag, TagDto>().ReverseMap();

            // NewsArticle -> NewsDto
            CreateMap<NewsArticle, NewsDto>()
                .ForMember(dest => dest.CategoryName,
                    opt => opt.MapFrom(src => src.Category != null ? src.Category.CategoryName : null))
                .ForMember(dest => dest.Tags,
                    opt => opt.MapFrom(src => src.Tags.Select(tag => tag.TagName ?? string.Empty).ToList()));

            // NewsCreateDto -> NewsArticle (gửi TagIds từ FE -> server)
            CreateMap<NewsCreateDto, NewsArticle>()
                .ForMember(dest => dest.Tags, opt => opt.Ignore()) // xử lý riêng ở service
                /*.ForMember(dest => dest.NewsArticleId, opt => opt.Ignore())*/
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore());
			CreateMap<AccountUpdateDto, SystemAccount>()
	            .ForMember(dest => dest.AccountPassword, opt => opt.Ignore()) // Không cập nhật password từ đây
	            .ForMember(dest => dest.AccountRole, opt => opt.Ignore());    // Không cập nhật role từ đây

			CreateMap<NewsUpdateDto, NewsArticle>()
				.ForMember(dest => dest.Tags, opt => opt.Ignore()) // xử lý ở Service
				.ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
				.ForMember(dest => dest.ModifiedDate, opt => opt.Ignore())
				.ForMember(dest => dest.CreatedById, opt => opt.Ignore())
				.ForMember(dest => dest.UpdatedById, opt => opt.Ignore());
		}
    }
}