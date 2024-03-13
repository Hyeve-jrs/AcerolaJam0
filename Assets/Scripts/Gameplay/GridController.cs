using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GridController : MonoBehaviour
{
	public Level initialLevel;
	public Vector2 realSpacing;
	public Tile[] objects;
	public Text hintText;

	public UnityEvent onLevelWon;
	
	private int busyCounter = 0;
	private Vector2Int currentLevelSize;
	private bool checkLevelWon = false;
	private bool allowInteractions = true;

	private Coroutine hintWritingRoutine;
	
	public void SetHintText(string hintText)
	{
		if(hintWritingRoutine != null) StopCoroutine(hintWritingRoutine);
		hintWritingRoutine = StartCoroutine(HintWriteRoutine(hintText));
	}

	private IEnumerator HintWriteRoutine(string text)
	{
		while (hintText.text.Length > 0)
		{
			hintText.text = hintText.text.Substring(0, hintText.text.Length - 1);
			AudioManager.PlaySfx(SfxType.TextClick);
			yield return new WaitForSeconds(0.05f);
		}

		yield return new WaitForSeconds(0.2f);
		
		for(int i = 0; i < text.Length; i++)
		{
			hintText.text += text[i];
			AudioManager.PlaySfx(SfxType.TextClick);
			yield return new WaitForSeconds(0.05f);
		}

		hintWritingRoutine = null;
	}
	
	public void TryInteract(int x, int y, Tile from)
	{
		if (x < 0 || x > currentLevelSize.x - 1) return;
		if (y < 0 || y > currentLevelSize.y - 1) return;
		int index = x + y * currentLevelSize.y;
		objects[index].Interacted(from);
		checkLevelWon = true;
	}

	public void Update()
	{
		if (checkLevelWon && CanActionsHappen() && IsWon())
		{
			onLevelWon.Invoke();
			checkLevelWon = false;
		}
	}

	public bool IsWon()
	{
		return objects.All(obj => obj.IsInWinState());
	}

	public bool CanActionsHappen()
	{
		return allowInteractions && objects.All(obj => obj.AllAnimsFinished());
	}

	public void SetCanInteract(bool allowInteract)
	{
		allowInteractions = allowInteract;
	}
	
	public void StartLevel(Level level)
	{
		StartCoroutine(InstantiateLevel(level));
	}

	private IEnumerator InstantiateLevel(Level level)
	{
		if (busyCounter > 0)
		{
			Debug.LogError("Grid controller is busy!");
			yield break;
		}

		busyCounter++;
		
		if (objects != null)
		{
			yield return StartCoroutine(DestroyLevel());
		}
		
		currentLevelSize = level.LevelSize();
		objects = new Tile[currentLevelSize.x * currentLevelSize.y];
		Vector2 baseOffset = new(currentLevelSize.x % 2 == 0 ? realSpacing.x * 0.5f : 0f, currentLevelSize.y % 2 == 0 ? realSpacing.y * 0.5f : 0f);
		
		for (int y = 0; y < currentLevelSize.y; y++)
		{
			for (int x = 0; x < currentLevelSize.x; x++)
			{
				int xRel = x - currentLevelSize.x / 2;
				int yRel = y - currentLevelSize.y / 2;
					
				int index = x + y * currentLevelSize.x;
				Vector2 position = baseOffset + realSpacing * new Vector2(xRel, yRel);
				objects[index] = level.Spawn(x, y, this, position, transform);
			}
		}
		
		yield return new WaitUntil(CanActionsHappen);
		
		busyCounter--;
	}

	public IEnumerator DestroyLevel()
	{
		busyCounter++;
		
		foreach (Tile gridObject in objects)
		{
			gridObject.Remove();
		}

		yield return new WaitUntil(CanActionsHappen);
		
		foreach (Tile gridObject in objects)
		{
			Destroy(gridObject.GetAssociated());
		}

		busyCounter--;
	}

#if UNITY_EDITOR
	[ContextMenu("Bake into level")]
	public void BakeLevel()
	{
		BakedLevel levelData = ScriptableObject.CreateInstance<BakedLevel>();
		levelData.size = currentLevelSize;

		TileData[] tileDatas = new TileData[objects.Length];
		for (int index = 0; index < objects.Length; index++)
		{
			Tile tile = objects[index];
			tileDatas[index] = tile.BakeData();
		}

		levelData.data = tileDatas;

		int id = Random.Range(0, int.MaxValue);
		AssetDatabase.CreateAsset(levelData, "Assets/BakedLevels/Level" + id + ".asset");
	}
#endif
}
