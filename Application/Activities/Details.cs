using Application.Core;
using Application.interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities
{
    public class Details
    {
        public class Query:IRequest<Result<ActivityDto>>{
            public Guid Id{ get; set; }
        }

        public class Handler : IRequestHandler<Query ,Result<ActivityDto>>{

        private readonly DataContext context;
        private readonly IMapper _mapper;
       
        private readonly IuserAccessor userAccessor;
       
            public Handler(DataContext context,IMapper mapper,IuserAccessor userAccessor)
            {
            this.userAccessor = userAccessor;
          
                 this.context = context;
                  _mapper = mapper;
            }
            public async Task<Result<ActivityDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var activity = await context.Activities
                .ProjectTo<ActivityDto>(_mapper.ConfigurationProvider,new { currentUsername = userAccessor.GetUsername() })
                .FirstOrDefaultAsync(x => x.Id == request.Id);

                return Result<ActivityDto>.Success(activity);
            }

            
        }
    }
}