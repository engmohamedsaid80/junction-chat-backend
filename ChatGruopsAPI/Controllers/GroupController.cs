using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatGruopsAPI.Models;
using DomainCore;
using Microsoft.AspNetCore.Mvc;

namespace ChatGroupsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        DomainCore.Interfaces.IDataRepo _repo;

        public GroupController(DomainCore.Interfaces.IDataRepo repo)
        {
            _repo = repo;
        }

        [HttpGet]
        [Route("GetAllGroups")]
        public async Task<IEnumerable<GroupViewModel>> GetAllGroups(string game)
        {
            CoreEngine core = new CoreEngine();

            var result = await core.GetGameGroupsAsync(_repo, TestingSettings.DEFAULT_GAME);

            IEnumerable<GroupViewModel> groups = from res in result
                                                 select new GroupViewModel
                                                 {
                                                     Name = res.Name,
                                                     Owner = res.Owner,
                                                     Game = res.Game
                                                 };
            return groups;
        }

        [HttpPost]
        [Route("CreateGroup")]
        public async Task<MessageResponse> CreateGroup(GroupViewModel group)
        {
            CoreEngine core = new CoreEngine();

            DomainCore.Models.GroupModel g = new DomainCore.Models.GroupModel
            {
                Game = TestingSettings.DEFAULT_GAME,
                Name = group.Name,
                Owner = group.Owner
            };

            var msg = await core.CreateGroup(_repo, g);

            var response = new MessageResponse { Message = msg };
            return response;
        }
    }
}