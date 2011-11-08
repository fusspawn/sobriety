var NativeClient = {};

NativeClient.OnChatMessage = function(message_string) {
     $("#UI_CHAT_MESSAGES").append("<li> " + message_string + "</li>");
     var message_count = $("#UI_CHAT_MESSAGES li").size();
     if(message_count > 5) {
        $("#UI_CHAT_MESSAGES li:first").remove();
     }
}

NativeClient.SendChatMessage = function() {
    var Input = $("#UI_CHAT_INPUT");
    var Message = Input.val();
    Input.val("");
    Native.SendChatMessage(Message);
}