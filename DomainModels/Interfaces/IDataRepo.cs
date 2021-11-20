using DomainCore.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DomainCore.Interfaces
{
    public interface IDataRepo
    {
        Task<string> AddGroupAsync(GroupModel group);
        Task<GroupModel> GetGroupAsync(string gid);
        Task<string> UpdateGroupAsync(GroupModel group);

        Task<IEnumerable<GroupModel>> GetGroupsAsync(string game);

        Task<UserModel> GetUser(string user);

        Task<string> UpdateUserAsync(UserModel user);

        Task<IEnumerable<MessageModel>> GetMessagesAsync(string group);

        Task<string> AddMessageAsync(MessageModel message);

    }
}
