using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BusinessObject.DTOs;
using BusinessObjects.Models;

namespace Service
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Map cho Account
            CreateMap<Account, AccountDTO>();
            CreateMap<AccountCreateDTO, Account>();
            CreateMap<AccountUpdateDTO, Account>();
            CreateMap<Account, UserDTO>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.RoleId == 1 ? "Admin" : (src.RoleId == 2 ? "User" : "Unknow")));

            // Map cho Pitch
            CreateMap<Pitch, PitchDTO>()
                .ForMember(dest => dest.PricePitch, opt => opt.MapFrom(src =>
                    src.PricePitches
                       .OrderByDescending(p => p.PricePitchId)
                       .FirstOrDefault().Price));

            CreateMap<PitchCreateDTO, Pitch>()
                .ForMember(dest => dest.PricePitches, opt => opt.Ignore());

            CreateMap<Pitch, PitchCreateDTO>();


            // Map cho TotalInvoice
            CreateMap<TotalInvoicePitch, TotalInvoicePitchDTO>()
                .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src =>
                    src.InvoicePitches.Sum(ip => ip.PricePitch.Price)));

            CreateMap<TotalInvoicePitchCreateDTO, TotalInvoicePitch>();
            CreateMap<TotalInvoicePitchUpdateDTO, TotalInvoicePitch>();


            // SỬA LỖI Ở ĐÂY: Gộp các quy tắc map cho TotalInvoiceWithDetailsDTO vào một chỗ
            CreateMap<TotalInvoicePitch, TotalInvoiceWithDetailsDTO>()
                .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src =>
                    src.InvoicePitches.Sum(ip => ip.PricePitch.Price)))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.User.Name)); // Sửa dest.Name thành dest.UserName


            // Map cho InvoicePitch (chi tiết hóa đơn)
            CreateMap<InvoicePitch, InvoicePitchItemDTO>()
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.PricePitch.Price))
                .ForMember(dest => dest.Time, opt => opt.MapFrom(src => src.BookingTime.Time));

            CreateMap<InvoicePitch, InvoicePitchDTO>()
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.PricePitch.Price))
                .ForMember(dest => dest.Time, opt => opt.MapFrom(src => src.BookingTime.Time));

            CreateMap<InvoicePitchCreateDTO, InvoicePitch>();
            CreateMap<InvoicePitchUpdateDTO, InvoicePitch>();


            //map cho Feedback
            CreateMap<FeedbackPitch, FeedbackPitchDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Name));
            CreateMap<FeedbackPitchCreateDto, FeedbackPitch>()
                .ForMember(dest => dest.TimeFeedback, opt => opt.MapFrom(src => DateOnly.FromDateTime(DateTime.Now)));
            CreateMap<FeedbackPitchUpdateDto, FeedbackPitch>();

            //Map cho bookingTime
            CreateMap<BusinessObjects.Models.BookingTime, BusinessObject.DTOs.BookingTimeDTO>();
        }
    }
}
