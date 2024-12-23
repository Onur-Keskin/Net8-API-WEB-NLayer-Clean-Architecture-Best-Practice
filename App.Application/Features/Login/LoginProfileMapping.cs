﻿using App.Domain.Entities;
using AutoMapper;

namespace App.Application.Features.Login
{
    public class LoginProfileMapping: Profile
    {
        public LoginProfileMapping()
        {
            CreateMap<LoginRequest, Domain.Entities.User>().ReverseMap();
        }
    }
}
