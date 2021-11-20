using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRChat.ChatAPIsModels
{
    public class UserInfoModel
    {
        public string UserName { get; set; }

        public string Game { get; set; }

        public IEnumerable<GroupNameModel> UserGroups { get; set; }

        public IEnumerable<GroupNameModel> OtherGroups { get; set; }

    }

    public class GroupNameModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
