using Mirror;
using MP.Common;
using MP.Manager;
using MP.Room.UI;
using UnityEngine;
using UnityEngine.UI;

namespace MP.Room.UI
{
    public class JoinUI : MonoBehaviour
    {
        [SerializeField] private GameObject _joinPanel;
        [SerializeField] private GameObject _rolePanel;
        [SerializeField] private GameObject _errorPanel;


        private void Start()
        {
            if (NetworkManager.singleton is NetworkRoomManagerExtended room)
            {
                room.OnConnectionError += OpenErrorPanel;
            }
        }

        private void OnDestroy()
        {
            if (NetworkManager.singleton is NetworkRoomManagerExtended room)
            {
                room.OnConnectionError -= OpenErrorPanel;
            }
        }

        public void ActivateUI()
        {
            _joinPanel.SetActive(true);
            _rolePanel.SetActive(true);
        }

        public void EndRoleChoice(bool isHost)
        {
            _rolePanel.SetActive(false);
            if (isHost)
            {
                EndJoin();
                return;
            }

            SetDefaultIPAdress();
        }

        public void EndJoin()
        {
            _joinPanel.SetActive(false);
        }

        private void OpenErrorPanel()
        {
            _errorPanel.SetActive(true);
        }

        private void SetDefaultIPAdress()
        {
            NetworkManager.singleton.networkAddress = "localhost";
        }

    }
}

