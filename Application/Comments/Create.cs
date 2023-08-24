using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using MediatR;
using FluentValidation;
using Persistence;
using AutoMapper;
using Application.interfaces;
using SQLitePCL;
using Microsoft.EntityFrameworkCore;
using Domain;

namespace Application.Comments
{
    public class Create
    {
        public class Command : IRequest<Result<CommentDto>>
        {
            public string Body { get; set; }
            public Guid ActivityId { get; set; }
        }
        public class  CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Body).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command, Result<CommentDto>>
        {
        private readonly DataContext _DataContext ;
        private readonly IMapper _mapper;
        private readonly IuserAccessor _UserAccessor ;
            public Handler(DataContext dataContext , IMapper mapper,IuserAccessor userAccessor)
            {
            _UserAccessor = userAccessor;
            _mapper = mapper;
            _DataContext = dataContext;
            }

            public async Task<Result<CommentDto>> Handle(Command request, CancellationToken cancellationToken)
            {
              var activity = await _DataContext.Activities.FindAsync(request.ActivityId);

              if(activity == null) return null;

              var user = await _DataContext.Users.Include(p => p.Photos).SingleOrDefaultAsync(x=>x.UserName == _UserAccessor.GetUsername());

              var comment = new Comment {
                Author = user,
                Activity = activity,
                Body = request.Body
              };
              activity.Comments.Add(comment);
              var success = await _DataContext.SaveChangesAsync() > 0;

            if(success) return Result<CommentDto>.Success(_mapper.Map<CommentDto>(comment));

            return Result<CommentDto>.Failure("Failed to add comment");
            }
        }
    }

}