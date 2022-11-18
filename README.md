# MultiPull
 Pulling Players game. Made with Unity and Mirror Networking

## Project State Note

After game scene restart only start position for isOwned Player is updated.
So for correct view all players must have performed at least one move in scene.

You may see it in NetworkGamePlayer.cs Script:

```
public void OnClientEnterGame()
        {
            //update owner only
            if (isOwned)
            {
                transform.position = _movement.StartPosition;
                //Debug.Log($"OWNER : Change position for {_stats.PlayerName}: " +
                //                $" new start position {_movement.StartPosition} and actual position is {transform.position}.");
            }
        }
```


## Made by
Valentina Khudyakova

## Notes
Parameters can be changed using UnityEditor in a "Data" Folder. 
Parameters are listed in the scriptable objects.
