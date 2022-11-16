using Mirror;
using MP.Room.UI;
using UnityEngine;

namespace MP.Room.View
{
    public class JoinLobbyAsAClient : IJoinLobby
    {
        private NetworkManager _networkManager;
        private JoinUI _joinUI;

        public JoinLobbyAsAClient(NetworkManager networkManager, JoinUI joinUI)
        {
            _joinUI = joinUI;
            _networkManager = networkManager;
        }

        public void JoinLobby()
        {
            _joinUI.EndRoleChoice(false);
            _networkManager.StartClient();
            _joinUI.EndJoin();
        }
    }
}

