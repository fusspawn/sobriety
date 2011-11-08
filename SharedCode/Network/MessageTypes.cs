using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharedCode.Network
{
    public enum MessageTypes : byte
    {
        ConnectionOkay = 0,
        ConnectionRefused = 1,
        RequestPlayerName = 2,
        PlayerNameResponse = 3,
        IncomingMapData = 4,
        MapDataChange = 5,
        PlayerMoveInput = 6,
        PlayerInteract = 7,
        ChatMessage = 8, 
        RemoteLog = 9,
        ActorMovement = 10,
        ActorCreate = 11,
        ActorDestroy = 12,
        ActorAnimate = 13,
        CameraFocusAt = 14,
        ObjectCreate = 15,
        ObjectMove = 16,
        ObjectInteract = 17,
        AttributeSync = 18,
        
    }
}
