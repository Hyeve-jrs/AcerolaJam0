using System;
using UnityEngine;
using Object = UnityEngine.Object;

[Serializable]
public class FlipTileData
{
	public TileConfigData config;
	public bool startsActive;
	public bool activeForWin;
	public Vector2Int[] flipOffsets;
	public bool glitched;

	public Tile Spawn(int x, int y, GridController controller, Vector2 position, Transform parent)
	{
		GameObject obj = Object.Instantiate(config.flipTilePrefab, parent);
		obj.GetComponent<RectTransform>().localPosition = position;
		FlipTile gridObj = obj.GetComponentInChildren<FlipTile>();
		gridObj.Assign(x, y, controller);
		gridObj.flipOffsets = flipOffsets;
		gridObj.startsActive = startsActive;
		gridObj.needsToBeActiveForWin = activeForWin;
		gridObj.glitched = glitched;
		return gridObj;
	}
}