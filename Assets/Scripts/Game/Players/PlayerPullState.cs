using Mirror;


namespace MP.Game.Players
{
    /// <summary>
    /// Component to store and sync PullState.
    /// <para>SyncVar: InAPull.</para>
    /// <para>CmdInAPullValue to change pull state.</para>
    /// </summary>
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

