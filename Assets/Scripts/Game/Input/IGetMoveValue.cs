using UnityEngine;

namespace MP.View.Input.Interfaces
{
    public interface IGetMoveValue
    {
        //delegate void MovePerformed(Vector2 delta);
        //event MovePerformed OnMovePerformed;
        Vector2 GetMoveValue();
    }

    public interface IPullPerformed
    {
        delegate void PullPerformed();
        event PullPerformed OnPullPerformed;
    }
}
