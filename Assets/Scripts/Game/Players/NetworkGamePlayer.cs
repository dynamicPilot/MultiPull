using Mirror;
using MP.Manager;


namespace MP.Common.Players
{
    /// <summary>
    /// This is a main component for Player data in a Game.
    /// <para>SyncVar: PlayerName.</para>
    /// </summary>
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

