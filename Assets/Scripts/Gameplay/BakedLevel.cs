using System;
using UnityEngine;


public class BakedLevel : Level
{
	public TileData[] data;
	public Vector2Int size;
	
	public override Vector2Int LevelSize()
	{
		return size;
	}

	public override Tile Spawn(int x, int y, GridController controller, Vector2 position, Transform parent)
	{
		return data[x + y * size.y].Spawn(x, y, controller, position, parent);
	}
}