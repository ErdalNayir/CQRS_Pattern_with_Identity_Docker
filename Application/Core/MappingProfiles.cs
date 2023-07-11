using Application.Activities;
using AutoMapper;
using Domain.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core
{
    public class MappingProfiles:Profile
    {
        public MappingProfiles()
        {
            CreateMap<Activity, Activity>();
            CreateMap<Activity, ActivityDto>()
                .ForMember(d=>d.HostUsername, o=>o.MapFrom(s=>s.Attendees
                .FirstOrDefault(x=>x.IsHost).AppUser.UserName));

            CreateMap<ActivityAttendee,AttendeeDto>()
                .ForMember(x=>x.DisplayName, o=>o.MapFrom(a=>a.AppUser.DisplayName))
				.ForMember(x => x.Bio, o => o.MapFrom(a => a.AppUser.Bio))
				.ForMember(x => x.Username, o => o.MapFrom(a => a.AppUser.UserName))
                .ForMember(x => x.Image, o => o.MapFrom(a => a.AppUser.Photos.FirstOrDefault(y => y.IsMain).Url));

			CreateMap<AppUser, Profiles.cs.Profile>()
                .ForMember(x=>x.Image, o=>o.MapFrom(a=>a.Photos.FirstOrDefault(y=>y.IsMain).Url));
        }
    }
}
