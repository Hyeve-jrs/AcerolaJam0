using UnityEngine;

[CreateAssetMenu(menuName = "Create DefaultLevel", fileName = "DefaultLevel", order = 0)]
public class BaseLevel : Level
{
	public Vector2Int size;
	public GameObject objectPrefab;
	
	public override Vector2Int LevelSize()
	{
		return size;
	}

	public override Tile Spawn(int x, int y, GridController controller, Vector2 position, Transform parent)
	{
		GameObject obj = Instantiate(objectPrefab.gameObject, parent);
		obj.GetComponent<RectTransform>().localPosition = position;
		Tile gridObj = obj.GetComponentInChildren<Tile>();
		gridObj.Assign(x, y, controller);
		return gridObj;
	}
}