using System;
using UnityEngine;

[Serializable]
public class TileData
{
	//I hate this setup, but unity doesn't support deserializing inherited serializable objects - it MUST be a known class
	//So, we have a single serializable class that contains (potential) references to the actual individual types,
	//and manually runs through to determine which to call.
	
	public FlipTileData flipTile;
	public Tile Spawn(int x, int y, GridController controller, Vector2 position, Transform parent)
	{
		if (flipTile != null)
		{
			return flipTile.Spawn(x, y, controller, position, parent);
		}

		return null;
	}
}