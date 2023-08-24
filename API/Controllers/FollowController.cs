using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Application.Followers;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class FollowController:BaseApiController
    {
        [HttpPost("{username}")]
        public async Task<IActionResult> Follow (string username)
        {
            return HandleResult(await Mediator.Send(new FollowToggle.Command{TargetUser = username}));
        }
  [HttpGet("{username}")]

  public async Task<IActionResult> getFollowing(string username, string Predicate)
  {
    return HandleResult(await Mediator.Send(new List.Query{Username = username,Predicate = Predicate}));
  }

    }
}