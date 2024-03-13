using UnityEngine;

public abstract class Level : ScriptableObject
{
	public abstract Vector2Int LevelSize();
	public abstract Tile Spawn(int x, int y, GridController controller, Vector2 position, Transform parent);
}