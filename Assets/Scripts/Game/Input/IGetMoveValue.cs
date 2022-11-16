using UnityEngine;

namespace MP.Game.Input.Interfaces
{
    public interface IGetMoveValue
    {
        Vector2 GetMoveValue();
    }

    public interface IPullPerformed
    {
        delegate void PullPerformed();
        event PullPerformed OnPullPerformed;
    }
}
