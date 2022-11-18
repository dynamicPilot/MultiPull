using Mirror;
using MP.Manager;
using MP.Room.UI;
using System;
using UnityEngine;

namespace MP.Room.View
{
    public class JoinLobbyAsAHost: IJoinLobby
    {
        private NetworkRoomManagerExtended _networkManager;
        private JoinUI _joinUI;

        public JoinLobbyAsAHost(NetworkRoomManagerExtended networkManager, JoinUI joinUI)
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

