# MultiPull
 Pulling Players game. Made with Unity and Mirror Networking

## Project State Note

After game scene restart only start position for isOwned Player is updated.
So for correct view all players must have performed at least one move in scene.

You may see it in NetworkGamePlayer.cs Script:

```c#
public void OnClientEnterGame()
        {
            //update owner only
            if (isOwned)
            {
                transform.position = _movement.StartPosition;
            }
        }
```


## Made by
Valentina Khudyakova

## Notes
Parameters can be changed using UnityEditor in a "Data" Folder. 
Parameters are listed in the scriptable objects.
