using UnityEngine;

public abstract class Tile : MonoBehaviour
{
	public abstract GameObject GetAssociated();
	public abstract void Assign(int x, int y, GridController controller);
	public abstract bool IsInWinState();
	public abstract void Clicked();
	public abstract void Interacted(Tile by);
	public abstract bool AllAnimsFinished();
	public abstract void Remove();

	public abstract TileData BakeData();
}