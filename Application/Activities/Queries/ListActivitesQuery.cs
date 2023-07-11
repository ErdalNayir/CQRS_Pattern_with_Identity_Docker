using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Concrete;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistance.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Activities.Queries
{
    public class ListActivitesQuery
    {
        public class Query : IRequest<Result<List<ActivityDto>>> { }

        public class Handler : IRequestHandler<Query, Result<List<ActivityDto>>>
        {
            private readonly DataContext _dataContext;
			private readonly IMapper _mapper;

			public Handler(DataContext dataContext,IMapper mapper)
            {
                _dataContext = dataContext;
				_mapper = mapper;
			}

            public async Task<Result<List<ActivityDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var activities = await _dataContext.Activities.ProjectTo<ActivityDto>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);

               // var activitiesToReturn = _mapper.Map<List<ActivityDto>>(activities);

				return Result<List<ActivityDto>>.Success(activities);
            }
        }
    }
}
