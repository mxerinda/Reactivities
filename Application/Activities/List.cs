
using MediatR;
using Persistence;
using Microsoft.EntityFrameworkCore;
using Domain;
using Microsoft.Extensions.Logging;
using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Application.interfaces;

namespace Application.Activities
{
    public class List
    {
        public class Query:IRequest<Result<PagedList<ActivityDto>>>
        {
            public ActivityParams Params {get; set;}
        }

        public class Handler : IRequestHandler<Query, Result<PagedList<ActivityDto>>>
        {
            private readonly DataContext context;
           
            private readonly IMapper mapper;
           private readonly IuserAccessor userAccessor;

            public Handler(DataContext context,IMapper mapper,IuserAccessor userAccessor)
            {
            this.userAccessor = userAccessor;
               this.mapper = mapper;
        
                this.context = context;
            }
            public async Task<Result<PagedList<ActivityDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                       var query =  context.Activities
                       .Where(d=>d.Date>= request.Params.StartDate)
                       .OrderBy(d =>d.Date)
                      .ProjectTo<ActivityDto>(mapper.ConfigurationProvider, new { currentUsername = userAccessor.GetUsername() })
                      .AsQueryable();

                      if(request.Params.IsGoing && !request.Params.IsHost)
                      {
                        query =query.Where(x=>x.Attendees.Any(a =>a.Username ==userAccessor.GetUsername()));
                      }

                      if(request.Params.IsHost && !request.Params.IsGoing)
                      {
                        query = query.Where(x=>x.HostUsername == userAccessor.GetUsername());
                      }

                 
                   
                return  Result<PagedList<ActivityDto>>.Success(
                    await PagedList<ActivityDto>.CreateAsync(query,request.Params.PageNumber,
                    request.Params.PageSize)
                );
            }
        }
    }
}