using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatGroupsAPI.Models;
using ChatGruopsAPI.Models;
using DomainCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatGroupsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        DomainCore.Interfaces.IDataRepo _repo;

        public UserController(DomainCore.Interfaces.IDataRepo repo)
        {
            _repo = repo;
        }

        [HttpGet]
        [Route("GetUserGroups")]
        public async Task<UserViewModel> GetUserGroups(string user)
        {
            CoreEngine core = new CoreEngine();
            var userObj = await core.GetUserGroupsAsync(_repo, TestingSettings.DEFAULT_GAME, user);

            UserViewModel userGroups = new UserViewModel
            {
                UserName = userObj.UserName,
                Game = userObj.Game,
                UserGroups = from ug in userObj.UserGroups select new GroupNameViewModel { Id = ug.Id, Name = ug.Name},
                OtherGroups = from og in userObj.OtherGroups select new GroupNameViewModel { Id = og.Id, Name = og.Name },
            };

            return userGroups;
        }

        [HttpPost]
        [Route("JoinGroup")]
        public async Task<MessageResponse> JoinGroup(UserGroupActionModel action)
        {
            CoreEngine core = new CoreEngine();

            var msg = await core.JoinGroup(_repo, action.User, action.Group);

            var response = new MessageResponse { Message = msg };

            return response;
        }

        [HttpPost]
        [Route("LeaveGroup")]
        public async Task<MessageResponse> LeaveGroup(UserGroupActionModel action)
        {
            CoreEngine core = new CoreEngine();

            var msg = await core.LeaveGroup(_repo, action.User, action.Group);

            var response = new MessageResponse { Message = msg };

            return response;
        }

        [HttpPost]
        [Route("UserLogin")]
        public async Task<MessageResponse> UserLogin(UserLoginRequest user)
        {
            CoreEngine core = new CoreEngine();

            var msg = await core.UserLogin(_repo, user.UserName, TestingSettings.DEFAULT_COUNTRY);

            var response = new MessageResponse { Message = msg };

            return response;
        }


    }
}