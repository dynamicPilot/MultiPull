using Mirror;
using MP.Room.UI;
using UnityEngine;

namespace MP.Room.View
{
    public class JoinLobbyAsAHost: IJoinLobby
    {
        private NetworkManager _networkManager;
        private JoinUI _joinUI;

        public JoinLobbyAsAHost(NetworkManager networkManager, JoinUI joinUI)
        {
            _joinUI = joinUI;
            _networkManager = networkManager;
        }

        public void JoinLobby()
        {
            _networkManager.StartHost();
            _joinUI.EndRoleChoice(true);
        }
    }
}

