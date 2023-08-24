using Application.Core;
using AutoMapper;
using MediatR;
using Persistence;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Application.interfaces;

namespace Application.Followers
{
    public class List
    {
        public class Query : IRequest<Result<List<Profiles.Profile>>>
        {
            public string Predicate { get; set; }
            public string Username { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<Profiles.Profile>>>
        {
            private readonly IMapper _mapper;
            private readonly DataContext _Context;
            private readonly IuserAccessor _UserAccessor;
            public Handler(DataContext context, IMapper mapper, IuserAccessor userAccessor)
            {
                _UserAccessor = userAccessor;
                _Context = context;
                _mapper = mapper;

            }


            public async Task<Result<List<Profiles.Profile>>> Handle(Query request, CancellationToken cancellationToken)
            {

                var profile = new List<Profiles.Profile>(); //Gettting  the profiles

                switch (request.Predicate)
                {
                    case "followers":
                        profile = await _Context.UserFollowings.Where(x => x.Target.UserName == request.Username).
                        Select(u => u.Observer)
                        .ProjectTo<Profiles.Profile>(_mapper.ConfigurationProvider,new {currentUsername = _UserAccessor.GetUsername()})
                        .ToListAsync();
                        break;

                    case "following":
                        profile = await _Context.UserFollowings.Where(x => x.Observer.UserName == request.Username).
                        Select(u => u.Target)
                        .ProjectTo<Profiles.Profile>(_mapper.ConfigurationProvider,new {currentUsername = _UserAccessor.GetUsername()})
                        .ToListAsync();
                        break;


                }
                return Result<List<Profiles.Profile>>.Success(profile);
            }
        }
    }
}