# MultiPull
 Pulling Players game. Made with Unity and Mirror Networking

## Project State Note (!)

After the game scene restart only start position for the isOwned Player is updated.
So for the correct view all players must have performed at least one move in the scene.

You may see it in *NetworkGamePlayer.cs* script:

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
