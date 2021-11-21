using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using SignalRChat.ChatAPIsModels;

namespace SignalRChat.SignalRHub
{
    public class ChatHub : Hub
    {

        
        string loginAPIUrl = ChatHubSettings.LOGIN_API;
        string userInfoUrl = ChatHubSettings.USER_INFO_API;
        string userScoreUrl = ChatHubSettings.USER_SCORE_API;
        string setUserScoreUrl = ChatHubSettings.SET_USER_SCORE_API;
        string createGroupUrl = ChatHubSettings.CREATE_GROUP_API;
        string joinGroupUrl = ChatHubSettings.JOIN_GROUP_API;
        string leavGroupUrl = ChatHubSettings.LEAVE_GROUP_API;
        string getGroupMessagesUrl = ChatHubSettings.GET_GROUP_MESSAGES_API;
        string sendMessageUrl = ChatHubSettings.SEND_MESSAGE_API;
        string messageGuardUrl = ChatHubSettings.MESSAGE_GUARD_API;


        public async Task SendMessageToGroup(string groupName, string message)
        {
            var senderName = Context.GetHttpContext().Request.Query["UserName"];

            HttpClient httpClient = new HttpClient();

            var isMessageSafe = await CallMessageGuard(httpClient, message);

            var score = await GetUSerScore(httpClient, senderName);

            if (isMessageSafe)
            {
                await Clients.Group(groupName).SendAsync("SendToGroup", $"{message}", groupName, senderName, score);

                await CallMessageAPI(httpClient, senderName, groupName, message);
            }     
            else
            {
                // Message is not safe
                score = (score != "3") ? (int.Parse(score) + 1).ToString() : "3";

                // 1- send private warning to sender
                await Clients.Group(groupName).SendAsync("SendToGroup", "message is not safe", groupName, senderName, score);

                // 2- lower sender score
                await SetUSerScore(httpClient, senderName, score);



            }
        }

        private async Task CallMessageAPI(HttpClient httpClient, string senderName, string groupName, string message)
        {
            //calling send message to group API

            ChatMessageModel messageVM = new ChatMessageModel();
            messageVM.Content = message;
            messageVM.SenderName = senderName;
            messageVM.GroupName = groupName;
            messageVM.MessageDT = System.DateTime.Now;

            var messagePayload = JsonConvert.SerializeObject(messageVM);
            // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
            var messagehttpContent = new StringContent(messagePayload, Encoding.UTF8, "application/json");

            var messageGroupUri = new Uri(string.Format(sendMessageUrl, string.Empty));

            await httpClient.PostAsync(messageGroupUri, messagehttpContent);
        }

        private async Task<string> GetUSerScore(HttpClient httpClient, string user)
        {
            var getUserScoreuri = new Uri(string.Format(userScoreUrl + user, string.Empty));
            var response = await httpClient.GetAsync(getUserScoreuri);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var scoreResponse = JsonConvert.DeserializeObject<APIMessageResponse>(content);
                return scoreResponse.Message;
            }

            return null;
        }

        private async Task SetUSerScore(HttpClient httpClient, string user, string score)
        {
            var setUserScoreUri = new Uri(string.Format(setUserScoreUrl + user + "&score=" + score, string.Empty));
            await httpClient.GetAsync(setUserScoreUri);
        }

        private async Task<bool> CallMessageGuard(HttpClient httpClient, string message)
        {
            var messageGuardUri = new Uri($"{messageGuardUrl}?message={message}");
            var response = await httpClient.GetAsync(messageGuardUri);

            if(response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var guardResponse = JsonConvert.DeserializeObject<MessageGuardResponse>(content);
                return guardResponse.Safe;
            }

            return false;
        }

        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            //await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} has joined the group {groupName}.");
            await Clients.Group(groupName).SendAsync("SendToGroup", $"{Context.ConnectionId}has joined the group {groupName}.", groupName, groupName);

            //calling Add to group API
            ChatGroupModel group = new ChatGroupModel();
            group.Name = groupName;
            group.Owner = Context.GetHttpContext().Request.Query["UserName"];

            var groupPayload = JsonConvert.SerializeObject(group);
            // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
            var grouphttpContent = new StringContent(groupPayload, Encoding.UTF8, "application/json");

            var createGroupUri = new Uri(string.Format(createGroupUrl, string.Empty));
            HttpClient groupClient = new HttpClient();
            var groupResponse = await groupClient.PostAsync(createGroupUri, grouphttpContent);


        }

        public async Task JoinGroup(string groupName)
        {
            //calling Join group API
            ChatUserGroupModel joinGroup = new ChatUserGroupModel();
            joinGroup.Group = groupName;
            joinGroup.User = Context.GetHttpContext().Request.Query["UserName"];

            var joinGroupPayload = JsonConvert.SerializeObject(joinGroup);
            // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
            var joinGrouphttpContent = new StringContent(joinGroupPayload, Encoding.UTF8, "application/json");

            var joineGroupUri = new Uri(string.Format(joinGroupUrl, string.Empty));
            HttpClient joinGroupClient = new HttpClient();
            var joinGroupResponse = await joinGroupClient.PostAsync(joineGroupUri, joinGrouphttpContent);

            if(joinGroupResponse.IsSuccessStatusCode)
            {
                var contentResult = await joinGroupResponse.Content.ReadAsStringAsync();
                var responseMessage = JsonConvert.DeserializeObject<APIMessageResponse>(contentResult);
                if(responseMessage.Message.StartsWith("error"))
                {
                    await Clients.Caller.SendAsync("DisplayError", responseMessage.Message);
                }
                else
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

                    //await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} has joined the group {groupName}.");
                    await Clients.Group(groupName).SendAsync("SendToGroup", $"{Context.GetHttpContext().Request.Query["UserName"]} is now online {groupName}.", groupName, groupName);

                    await Clients.Caller.SendAsync("JoinGroupTab", groupName);

                    //calling get group messages API
                    getGroupMessagesUrl = getGroupMessagesUrl + groupName;

                    var groupMessageUri = new Uri(string.Format(getGroupMessagesUrl, string.Empty));
                    HttpClient getGroupMessagesClient = new HttpClient();
                    var getGroupMessagesResponse = await getGroupMessagesClient.GetAsync(groupMessageUri);


                    var messagesContent = await getGroupMessagesResponse.Content.ReadAsStringAsync();

                    IEnumerable<ChatMessageModel> groupMessagesVM = await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<ChatMessageModel>>(messagesContent.ToString()));

                    foreach (ChatMessageModel msgVM in groupMessagesVM)
                    {

                        await Clients.Caller.SendAsync("PopulateMyGroupMessage", msgVM.GroupName, msgVM.SenderName, msgVM.Content);
                    }
                }
                

            }




        }

        public async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);

            await Clients.Group(groupName).SendAsync("SendToGroup", $"{Context.ConnectionId} has left the group {groupName}.", groupName, groupName);

            //calling remove from group API
            ChatUserGroupModel userGroup = new ChatUserGroupModel();
            userGroup.Group = groupName;
            userGroup.User = Context.GetHttpContext().Request.Query["UserName"];

            var userGroupPayload = JsonConvert.SerializeObject(userGroup);
            // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
            var userGrouphttpContent = new StringContent(userGroupPayload, Encoding.UTF8, "application/json");

            var leaveGroupUri = new Uri(string.Format(leavGroupUrl, string.Empty));
            HttpClient leaveGroupClient = new HttpClient();
            var leaveGroupResponse = await leaveGroupClient.PostAsync(leaveGroupUri, userGrouphttpContent);


        }

        public Task SendPrivateMessage(string user, string message)
        {
            return Clients.User(user).SendAsync("ReceiveMessage", message);
        }

        public override async Task OnConnectedAsync()
        {
            try
            {
                await base.OnConnectedAsync();

                string userName = Context.GetHttpContext().Request.Query["UserName"];

                //calling login/register user api
                ChatUserLoginRequest user = new ChatUserLoginRequest();
                user.UserName = userName;
                var userLoginPayload = JsonConvert.SerializeObject(user);
                // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
                var userLoginhttpContent = new StringContent(userLoginPayload, Encoding.UTF8, "application/json");
                
                loginAPIUrl += userName;
                var loginUri = new Uri(string.Format(loginAPIUrl, string.Empty));
                HttpClient loginClient = new HttpClient();
                var loginResponse = await loginClient.PostAsync(loginUri, userLoginhttpContent);


                //calling get user Groups and all groups api

                userInfoUrl += userName ;
                var getUserGroupsuri = new Uri(string.Format(userInfoUrl, string.Empty));
                HttpClient getUserGroupsClient = new HttpClient();
                var getUserGroupsResponse = await getUserGroupsClient.GetAsync(getUserGroupsuri);

                UserInfoModel userGroupsVM = new UserInfoModel();

                if (getUserGroupsResponse.IsSuccessStatusCode)
                {
                    var content = await getUserGroupsResponse.Content.ReadAsStringAsync();

                    
                    userGroupsVM = await Task.Run(() => JsonConvert.DeserializeObject<UserInfoModel>(content.ToString()));
                    string groupNames = "";
                    IEnumerable<GroupNameModel> OtherGroups = userGroupsVM.OtherGroups;
                    foreach(GroupNameModel f in OtherGroups)
                    {
                        groupNames = groupNames + f.Name + ",";
                    }
                    //await Clients.User(Context.ConnectionId).SendAsync("PopulateGroupList", groupNames);
                    await Clients.Caller.SendAsync("PopulateGroupList", groupNames);


                    IEnumerable<GroupNameModel> myGroups = userGroupsVM.UserGroups;
                    foreach (GroupNameModel f in myGroups)
                    {
                        await Groups.AddToGroupAsync(Context.ConnectionId, f.Name);
                        await Clients.Caller.SendAsync("PopulateMyGroupList", f.Name);
                    }

                    foreach (GroupNameModel f in myGroups)
                    {
                        //calling get group messages API
                        string getGroupMessagesUrl2 = getGroupMessagesUrl + f.Name;

                        var groupMessageUri = new Uri(string.Format(getGroupMessagesUrl2, string.Empty));
                        HttpClient getGroupMessagesClient = new HttpClient();
                        var getGroupMessagesResponse = await getGroupMessagesClient.GetAsync(groupMessageUri);

                        //IEnumerable<MessageViewModel> groupMessagesVM = new IEnumerable<MessageViewModel>();
                        if (getGroupMessagesResponse.IsSuccessStatusCode)
                        {
                            var messagesContent = await getGroupMessagesResponse.Content.ReadAsStringAsync();

                            IEnumerable<ChatMessageModel> groupMessagesVM = await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<ChatMessageModel>>(messagesContent.ToString()));

                            foreach(ChatMessageModel msgVM in groupMessagesVM)
                            {
                                
                                 await Clients.Caller.SendAsync("PopulateMyGroupMessage", msgVM.GroupName,msgVM.SenderName,msgVM.Content);
                            }
                        }

                    }
                }


                await Clients.Caller.SendAsync("HideSpinner");

            }
            catch (Exception ex)
            {

            }
            
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
    }
}
