using AppUser.Dto.comment;
using AppUser.Dto.stock;
using AppUser.Models;
using AutoMapper;

namespace AppUser.Mapper
{
    public class MappingProfiles:Profile
    {
        public MappingProfiles()
        {
            CreateMap<Stock, StockDto>();//Get
            CreateMap<StockDto, Stock>();//Create,Update
            CreateMap<CreateStockDto, Stock>();//Create,Update
            CreateMap<UpdateStockDto, Stock>();//Create,Update


            CreateMap<Comment, CommentDto>();//Get
            CreateMap<CommentDto, Comment>();//Create,Update
            CreateMap<CreateCommentDto, Comment>();//Create,Update
            CreateMap<UpdateCommentDto, Comment>();//Create,Update
        }
    }
}
