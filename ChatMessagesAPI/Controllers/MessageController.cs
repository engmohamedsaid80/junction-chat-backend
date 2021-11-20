using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatMessagesAPI.Models;
using DomainCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatMessagesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        DomainCore.Interfaces.IDataRepo _repo;

        public MessageController(DomainCore.Interfaces.IDataRepo repo)
        {
            _repo = repo;
        }

        [HttpGet]
        [Route("GetGroupMessages")]
        public async Task<IEnumerable<MessageViewModel>> GetGroupMessages(string g)
        {
            CoreEngine core = new CoreEngine();

            var result = await core.GetGroupMessagesAsync(_repo, g);

            IEnumerable<MessageViewModel> messages = from res in result
                                                     select new MessageViewModel
                                                     {
                                                         SenderName = res.SenderName,
                                                         GroupName = res.GroupName,
                                                         Content = res.Content,
                                                         MessageDT = res.MessageDT
                                                     };
            return messages;
        }


        [HttpPost]
        [Route("SendToGroup")]
        public async Task<MessageResponse> SendToGroup(MessageViewModel message)
        {
            CoreEngine core = new CoreEngine();

            DomainCore.Models.MessageModel m = new DomainCore.Models.MessageModel
            {
                SenderName = message.SenderName,
                GroupName = message.GroupName,
                Content = message.Content,
                MessageDT = DateTime.Now,
                id = message.GroupName + DateTime.Now.ToString("yyyyMMddhhMIss")
            };

            var msg = await core.SendMessageToGroupAsync(_repo, m);

            var response = new MessageResponse { Message = msg };

            return response;
        }


    }
}