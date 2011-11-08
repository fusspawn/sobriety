 var UIItems = {};
    
$(function() {
    UIItems.UIContainer = $("#UI_CONTAINER");
    UIItems.UIChatContainer = $("#UI_CHAT_CONTAINER");
    UIItems.UIChatMessages = $("#UI_CHAT_MESSAGES");
    UIItems.UIChatInput = $("#UI_CHAT_INPUT");
    UIItems.UIChatSubmit = $("#UI_CHAT_SUBMIT");
    UIItems.UIChatScroll = $("#UI_CHAT_SCROLL");
    
    UIItems.UIChatInput.keypress(function(event) {
      if ( event.which == 13 ) {
         event.preventDefault();
         NativeClient.SendChatMessage();
      }
    });
});



