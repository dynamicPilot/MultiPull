using Mirror;
using MP.Manager;


namespace MP.Common.Players
{
    public class NetworkGamePlayer : NetworkBehaviour
    {
        [SyncVar]
        public string PlayerName;

        public void Start()
        {
            if (NetworkManager.singleton is NetworkRoomManagerExtended manager)
            {
                if (manager.dontDestroyOnLoad)
                    DontDestroyOnLoad(gameObject);
            }

        }
    }
}

