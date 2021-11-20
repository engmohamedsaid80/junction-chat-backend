// The following sample code uses modern ECMAScript 6 features 
// that aren't supported in Internet Explorer 11.
// To convert the sample for environments that do not support ECMAScript 6, 
// such as Internet Explorer 11, use a transpiler such as 
// Babel at http://babeljs.io/.
var userName;
var url = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
for (var i = 0; i < url.length; i++) {
    var urlparam = url[i].split('=');
    if (urlparam[0] == 'name') {
        userName = urlparam[1];
    }
}

var url = "https://ms-ng-chat-hub.azurewebsites.net/chatHub?UserName=" + userName;
const connection = new signalR.HubConnectionBuilder()
    .withUrl(url)
    .build();
connection.on("Send", function (message) {
    var li = document.createElement("li");
    li.textContent = message;
    document.getElementById("messagesList").appendChild(li);
});

connection.on("DisplayError", function (message) {
    alert(message);
});

connection.on("JoinGroupTab", function (groupName) {
        var AnchorId = "Anchor_" + groupName;
        var liId = "li_" + groupName;

        var groupLi = "<li role='presentation' id ='li_" + groupName + "' class='active' onclick='ActiveGroup(this.id);'>" +
            "<a href='#" + groupName + "'  data-toggle='tab' id='" + AnchorId + "'><div class='chatInfoWrapper'>" +
            "<div class='chatUserAvatar'><img src='/ChatStyle/images/defaultAvatar.png' alt='User Avatar'>" +
            "</div><div class='chatUserInfo'><h3>" + groupName + "</h3></div></div></a></li>";

        $("#ui_Groups").append(groupLi);
        var myId = "chatScroll_" + groupName;
        var chatBox = "<div role='tabpanel' class='tab-pane' id='" + groupName + "'><div class='chatScroll' id='" + myId + "'>" + groupName + "</div></div>";

        $("#chatBox").append(chatBox);

        $("li").removeClass('active');
        $("#" + liId).addClass('active');
        $("#" + AnchorId).trigger("click");
});

connection.on("SendToGroup", function (message,group, senderName) {
    //var li = document.createElement("li");
    //li.textContent = message;
    //document.getElementById("messagesList").appendChild(li);

    var chatMessage = "<div class='messageWrapper'><div class='messageAvatar'>" +
        "<img src='/ChatStyle/images/defaultAvatar.png' alt='User Avatar'></div>" +
        "<div class='messageInfo'><div class='messageUser'><h3>" + senderName+"</h3>"+
        "</div><p>" +  message +"</p></div></div>";
    var chatScrollId = "chatScroll_" + group;
    $("#" + chatScrollId).append(chatMessage);
    //document.getElementById(chatScrollId).appendChild(chatMessage);
});


connection.on("PopulateGroupList", function (GroupNames) {
   
    var groupArray = GroupNames.split(",");
    for (i = 0; i < groupArray.length; i++) {

        $("#groupsDL").append($("<option     />").val(groupArray[i]).text(groupArray[i]));

    }

});

connection.on("PopulateMyGroupMessage", function (GroupName,SenderName,MsgContent) {

    var chatMessage = "<div class='messageWrapper'><div class='messageAvatar'>" +
        "<img src='/ChatStyle/images/defaultAvatar.png' alt='User Avatar'></div>" +
        "<div class='messageInfo'><div class='messageUser'><h3>" + SenderName + "</h3>"+
        "</div><p>" + MsgContent + "</p></div></div>";
    var chatScrollId = "chatScroll_" + GroupName;
    $("#" + chatScrollId).append(chatMessage);

});


connection.on("PopulateMyGroupList", function (groupName) {

    groupName = groupName.replace(/ /g, '-');

    var AnchorId = "Anchor_" + groupName;
    var liId = "li_" + groupName;
    

        var groupLi = "<li role='presentation' id ='li_" + groupName + "' class='active' onclick='ActiveGroup(this.id);'>" +
            "<a href='#" + groupName + "'  data-toggle='tab' id='" + AnchorId + "'><div class='chatInfoWrapper'>" +
            "<div class='chatUserAvatar'><img src='/ChatStyle/images/defaultAvatar.png' alt='User Avatar'>" +
            "</div><div class='chatUserInfo'><h3>" + groupName + "</h3></div></div></a></li>";

        $("#ui_Groups").append(groupLi);
        var myId = "chatScroll_" + groupName;
        var chatBox = "<div role='tabpanel' class='tab-pane' id='" + groupName + "'><div class='chatScroll' id='" + myId + "'>" + groupName + "</div></div>";

        $("#chatBox").append(chatBox);

        $("li").removeClass('active');
        $("#" + liId).addClass('active');
        $("#" + AnchorId).trigger("click");



});


connection.on("HideSpinner", function () {

    $('.spinnerWrapper').fadeOut('slow');
    

});

// We need an async function in order to use await, but we want this code to run immediately,
// so we use an "immediately-executed async function"
(async () => {
    try {
        await connection.start();
    }
    catch (e) {
        console.error(e.toString());
    }
})();

function ActiveGroup(id) {
    var groupName = id.substring(3);
    $('#ActiveGroup').val(groupName);
}


async function AddGroup() {
    //alert("addgroup");
    var groupName = document.getElementById("group-name").value;

    groupName = groupName.replace(/ /g, '-');

    
    try {

        var AnchorId = "Anchor_" + groupName;
        var liId = "li_" + groupName;

        var groupLi = "<li role='presentation' id ='li_" + groupName + "' class='active' onclick='ActiveGroup(this.id);'>" +
            "<a href='#" + groupName + "'  data-toggle='tab' id='" + AnchorId +"'><div class='chatInfoWrapper'>" +
            "<div class='chatUserAvatar'><img src='/ChatStyle/images/defaultAvatar.png' alt='User Avatar'>" +
            "</div><div class='chatUserInfo'><h3>" + groupName +"</h3></div></div></a></li>";

        $("#ui_Groups").append(groupLi);
        var myId = "chatScroll_"+groupName;
        var chatBox = "<div role='tabpanel' class='tab-pane' id='" + groupName + "'><div class='chatScroll' id='" + myId+"'>" + groupName+  "</div></div>";

        $("#chatBox").append(chatBox);

        $("li").removeClass('active');
        $("#" + liId).addClass('active');
        $("#" + AnchorId).trigger("click");

        //add this user to the new group
        await connection.invoke("AddToGroup", groupName);
    }
    catch (e) {
        console.error(e.toString());
    }
    event.preventDefault();
}

async function JoinGroup() {
    //alert("joingroup");
    var groupName = $('#groupsDL').val();

    groupName = groupName.replace(/ /g, '-');

    
    try {

        //add this user to the new group
        await connection.invoke("JoinGroup", groupName);
    }
    catch (e) {
        console.error(e.toString());
    }
    event.preventDefault();
}


async function SendMessage() {
    
    var groupName = document.getElementById("ActiveGroup").value;
    var groupMsg = document.getElementById("chatTextareas").value;
    document.getElementById("chatTextareas").value = "";
    try {
        await connection.invoke("SendMessageToGroup", groupName, groupMsg);
    }
    catch (e) {
        console.error(e.toString());
    }
    event.preventDefault();

}


async function LeavGroup() {
   
    var groupName = document.getElementById("ActiveGroup").value;
   
    document.getElementById("chatTextareas").value = "";

    $("#li_" + groupName).remove();
    $("#chatScroll_" + groupName).remove();

    try {
        await connection.invoke("RemoveFromGroup", groupName);
    }
    catch (e) {
        console.error(e.toString());
    }
    event.preventDefault();

}




