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
        public class Command :IRequest<Result<Unit>>
        {
            public string TargetUserName { get; set;}        
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
         private readonly DataContext Context;
        private readonly IuserAccessor userAccessor;

        public Handler (DataContext context,IuserAccessor userAccessor)
            {
            this.userAccessor = userAccessor;
            this.Context = context;
          
                
            }
            public  async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
               var observer = await Context.Users.FirstOrDefaultAsync(x =>
                x.UserName == userAccessor.GetUsername());

                var target = await Context.Users.FirstOrDefaultAsync(x =>
                x.UserName == request.TargetUserName);

                if(target == null) return null;

                var following =  await Context.UserFollowings.FindAsync(observer.Id,target.Id);

                if(following == null){

                    following = new UserFollowing
                    {
                        Observer = observer,
                        Target = target
                    };

                    Context.UserFollowings.Add(following);
                }
                else{
                    Context.UserFollowings.Remove(following);
                }

                var success = await Context.SaveChangesAsync() > 0;
                
                if(success) return Result<Unit>.Success(Unit.Value);

                return Result<Unit>.Failure("Failed to update following");
            }
        }
    }
}