using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRChat.SignalRHub
{
    public class ChatHubSettings
    {
        public static readonly string LOGIN_API = "https://ngchatgroupsapi.azurewebsites.net/api/user/UserLogin?user=";
        public static readonly string USER_INFO_API = "https://ngchatgroupsapi.azurewebsites.net/api/user/GetUserGroups?user=";
        public static readonly string CREATE_GROUP_API = "https://ngchatgroupsapi.azurewebsites.net/api/group/CreateGroup";
        public static readonly string JOIN_GROUP_API = "https://ngchatgroupsapi.azurewebsites.net/api/user/JoinGroup";
        public static readonly string LEAVE_GROUP_API = "https://ngchatgroupsapi.azurewebsites.net/api/user/LeaveGroup";
        public static readonly string GET_GROUP_MESSAGES_API = "https://ngchatmessagesapi.azurewebsites.net/api/Message/GetGroupMessages?g=";
        public static readonly string SEND_MESSAGE_API = "https://ngchatmessagesapi.azurewebsites.net/api/Message/SendToGroup";
    }
}
