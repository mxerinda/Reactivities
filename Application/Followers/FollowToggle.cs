using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Followers
{
    public class FollowToggle
    {
        public class Command : IRequest<Result<Unit>>
        {
            public string TargetUser { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {

            private readonly DataContext _context;
            private readonly IuserAccessor _userAccessor;
            public Handler(DataContext context, IuserAccessor userAccessor)
            {
                _context = context;
                _userAccessor = userAccessor;

            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var observer = await _context.Users.FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());
                var target = await _context.Users.FirstOrDefaultAsync(x => x.UserName == request.TargetUser);

                if (target == null) return null;

                var Following = await _context.UserFollowings.FindAsync(observer.Id, target.Id);

                if (Following == null)
                {
                    Following = new UserFollowing
                    {
                        Observer = observer,
                        Target = target,
                    };
                    _context.UserFollowings.Add(Following);
                }
                else{
                    _context.UserFollowings.Remove(Following);
                }
                var success = await _context.SaveChangesAsync() >0;

                if(success) return Result<Unit>.Success(Unit.Value);

                return Result<Unit>.Failure("Could not make a following");

            }
        }
    }


}