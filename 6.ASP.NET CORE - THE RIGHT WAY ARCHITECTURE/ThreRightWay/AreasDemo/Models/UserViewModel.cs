using AreasDemo.Data;
using AreasDemo.Infrastructure.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace AreasDemo.Models
{
    public class UserViewModel : IMapFrom<ApplicationUser>, IHaveCustomMapping
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string MailAddres { get; set; }

        public void ConfigureMapping(Profile profile)
            => profile.CreateMap<ApplicationUser, UserViewModel>()
            .ForMember(u => u.MailAddres, cfg => cfg.MapFrom(u => u.Email));
    }
}
