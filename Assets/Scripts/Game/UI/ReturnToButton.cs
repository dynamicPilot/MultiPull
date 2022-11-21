using Mirror;
using MP.Manager;

namespace MP.Game.UI
{
    /// <summary>
    /// UI component for Button OnClick action. 
    /// <para>Only for server: StopHostServer with NetworkRoomManagerExtended.</para>
    /// </summary>
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

