using App.Domain.Entities;
using AutoMapper;

namespace App.Application.Features.Register
{
    public class RegisterProfileMapping:Profile
    {
        public RegisterProfileMapping()
        {
            CreateMap<RegisterRequest, Domain.Entities.User>();
        }
    }
}
