using Application.Core;
using Application.Profiles.cs;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistance.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Profiles.Queries
{
	public class ProfileDetailQuery
	{
		public class Query : IRequest<Result<cs.Profile>>
		{
            public string Username { get; set; }
        }

		public class Handler : IRequestHandler<Query, Result<cs.Profile>>
		{
			private readonly DataContext _dataContext;
			private readonly IMapper _mapper;

			public Handler(DataContext dataContext, IMapper mapper)
            {
				_dataContext = dataContext;
				_mapper = mapper;
			}

            public async Task<Result<cs.Profile>> Handle(Query request, CancellationToken cancellationToken)
			{
				var user = await _dataContext.Users.ProjectTo<cs.Profile>(_mapper.ConfigurationProvider).SingleOrDefaultAsync(x=>x.Username == request.Username);

				return Result<cs.Profile>.Success(user);
			}
		}
	}
}
