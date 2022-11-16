using Mirror;
using MP.Room.UI;
using UnityEngine;

namespace MP.Room.View
{
    public class JoinLobbyAs : MonoBehaviour, IJoinLobby
    {
        [SerializeField] private JoinUI _joinUI;

        private IJoinLobby _joinLobby;
        public void JoinLobbyWithRole(bool isHost)
        {
            if (isHost) _joinLobby = new JoinLobbyAsAHost(NetworkManager.singleton, _joinUI);
            else _joinLobby = new JoinLobbyAsAClient(NetworkManager.singleton, _joinUI);
            JoinLobby();
        }

        public void JoinLobby()
        {
            _joinLobby.JoinLobby();
        }
    }
}
