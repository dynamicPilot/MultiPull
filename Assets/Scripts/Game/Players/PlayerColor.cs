using Mirror;
using MP.Common;
using UnityEngine;

namespace MP.Game.Players
{
    public class PlayerColor : NetworkBehaviour
    {
        [SerializeField] private StaticGamePlayerData _playerData;
        [SerializeField] private Renderer _playerBodyRender;

        [SyncVar(hook = nameof(SyncColor))]
        public Color Color;
        private Material _material;

        private void OnDestroy()
        {
            Destroy(_material);
        }

        public override void OnStartServer()
        {
            Color = _playerData.InitialColor;
            base.OnStartServer();
        }

        private void SyncColor(Color oldValue, Color newValue)
        {
            UpdateMaterial(newValue);
        }

        private void UpdateMaterial(Color newColor)
        {
            if (_material == null) _material = _playerBodyRender.material;
            _material.color = newColor;
        }
    }
}
