using Mirror;


namespace MP.Game.Players
{
    public class PlayerPullState : NetworkBehaviour
    {
        [SyncVar]
        public bool InAPull;

        private void OnValidate()
        {
            this.syncDirection = SyncDirection.ClientToServer;
        }

        [Command]
        public void CmdInAPullValue(bool newValue)
        {
            InAPull = newValue;
        }
    }
}

