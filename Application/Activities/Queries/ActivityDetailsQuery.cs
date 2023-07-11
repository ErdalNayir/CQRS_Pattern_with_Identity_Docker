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
    public class ActivityDetailsQuery
    {
        public class Query: IRequest<Result<ActivityDto>>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<ActivityDto>>
        {
            private DataContext _dataContext;
			private readonly IMapper _mapper;

			public Handler(DataContext dataContext,IMapper mapper)
            {
                _dataContext = dataContext;
				_mapper = mapper;
			}

            public async Task<Result<ActivityDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var activity =  await _dataContext.Activities.ProjectTo<ActivityDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(x=>x.Id == request.Id, cancellationToken);

                return Result<ActivityDto>.Success(activity);
            }
        }
    }
}
