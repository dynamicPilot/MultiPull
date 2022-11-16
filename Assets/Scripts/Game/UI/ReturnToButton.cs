using Mirror;
using MP.Manager;

namespace MP.Game.UI
{
    public class ReturnToButton : NetworkBehaviour
    {
        public void ReturnTo()
        {
            if (isServer)
            {
               if (NetworkManager.singleton is NetworkRoomManagerExtended room)
                    room.HostStopServer();
            }
        }
    }
}

