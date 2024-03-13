using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class FlipTile : BaseTile
{
	public bool startsActive;
	public bool needsToBeActiveForWin;
	public FlipPart[] parts; 
	public TileConfigData config;

	public Vector2Int[] flipOffsets;
	public Image[] flipOffsetImages;

	public bool glitched;
	public AnimationCurve glitchSlamInCurve;
	public Transform[] rgbGlitchedImages;
	
	private bool isActive;

	private bool isGlitchRunning;
	protected override void Start()
	{
		base.Start();
		SetActive(startsActive);
		UpdateDirectionImages();
		StartCoroutine(GlitchedImagesEffect());
	}

	private IEnumerator GlitchedImagesEffect()
	{
		if (!glitched)
		{
			foreach (Transform rgbGlitchedImage in rgbGlitchedImages)
			{
				rgbGlitchedImage.gameObject.SetActive(false);
			}

			yield break;
		}

		isGlitchRunning = true;
        
		foreach (Transform rgbGlitchedImage in rgbGlitchedImages)
		{
			rgbGlitchedImage.gameObject.SetActive(true);
		}
		
		Vector2 rOuterPos = new(-2000, 1000);
		Vector2 gOuterPos = new(2000, 1000);
		Vector2 bOuterPos = new(0, -2000);
		
		float delta = 1.0f;
		while (delta > 0.0f)
		{
			delta -= Time.deltaTime * 0.75f;
			float anim = glitchSlamInCurve.Evaluate(delta);
			rgbGlitchedImages[0].localPosition = rOuterPos * anim;
			rgbGlitchedImages[1].localPosition = gOuterPos * anim;
			rgbGlitchedImages[2].localPosition = bOuterPos * anim;
			yield return null;
		}

		Vector2 rInnerPosMax = new(-5, 3);
		Vector2 gInnerPosMax = new(5, 3);
		Vector2 bInnerPosMax = new(0, -10);
		
		while (true)
		{
			Vector2 randR = Random.insideUnitCircle;
			Vector2 randG = Random.insideUnitCircle;
			Vector2 randB = Random.insideUnitCircle;
			randR = new(Mathf.Abs(randR.x), Mathf.Abs(randR.y));
			randG = new(Mathf.Abs(randG.x), Mathf.Abs(randG.y));
			randB = new(Mathf.Abs(randB.x), Mathf.Abs(randB.y));
			
			
			rgbGlitchedImages[0].localPosition = rInnerPosMax * randR;
			rgbGlitchedImages[1].localPosition = gInnerPosMax * randG;
			rgbGlitchedImages[2].localPosition = bInnerPosMax * randB;

			yield return null;

			if (!glitched) break;
		}
		
		foreach (Transform rgbGlitchedImage in rgbGlitchedImages)
		{
			rgbGlitchedImage.gameObject.SetActive(false);
		}

		isGlitchRunning = false;
	}

	protected override void OnClicked()
	{
		owner.TryInteract(x, y, this);
		foreach (Vector2Int offset in flipOffsets)
		{
			owner.TryInteract(x + offset.x, y + offset.y, this);
		}
	}

	protected override void OnInteracted(Tile from)
	{
		StartCoroutine(FlipRoutine());
	}

	[ContextMenu("Set full direction offsets")]
	public void SetFullDirectionOffsets()
	{
		flipOffsets = new[]
		{
			Vector2Int.down, Vector2Int.left, Vector2Int.right, Vector2Int.up
		};
		UpdateDirectionImages();
	}

	private void OnValidate()
	{
		UpdateDirectionImages();
		if (glitched && !isGlitchRunning) StartCoroutine(GlitchedImagesEffect());
	}

	private IEnumerator FlipRoutine()
	{
		isAnimating = true;
		transform.localScale = Vector3.one;

		float timer = 0f;
		while (timer < flipAnimTime / 2f)
		{
			timer += Time.deltaTime;
			float alpha = timer / flipAnimTime;
			float scale = Mathf.Abs(1.0f - flipAnimCurve.Evaluate(alpha) * 2f);
			transform.localScale = new Vector3(1, scale, 1);
			yield return null;
		}
		
		SetActive(!isActive);
		
		while (timer < flipAnimTime)
		{
			timer += Time.deltaTime;
			float alpha = timer / flipAnimTime;
			float scale = Mathf.Abs(1.0f - flipAnimCurve.Evaluate(alpha) * 2f);
			transform.localScale = new Vector3(1, scale, 1);
			yield return null;
		}
		
		transform.localScale = Vector3.one;
		isAnimating = false;
	}
	
	private void SetActive(bool active)
	{
		isActive = active;
		foreach (FlipPart flipPart in parts)
		{
			flipPart.SetActive(active);
		}
	}

	public override bool IsInWinState()
	{
		return needsToBeActiveForWin ? isActive : !isActive;
	}

	public override TileData BakeData()
	{
		FlipTileData data = new()
		{
			flipOffsets = flipOffsets,
			startsActive = isActive,
			activeForWin = needsToBeActiveForWin,
			glitched = glitched,
			config = config
		};
		return new TileData()
		{
			flipTile = data
		};
	}

	public void UpdateDirectionImages()
	{
		foreach (Image flipOffsetImage in flipOffsetImages)
		{
			flipOffsetImage.enabled = false;
		}

		foreach (Vector2Int offset in flipOffsets)
		{
			if (offset == Vector2Int.left) flipOffsetImages[0].enabled = true;
			if (offset == Vector2Int.right) flipOffsetImages[1].enabled = true;
			if (offset == Vector2Int.up) flipOffsetImages[2].enabled = true;
			if (offset == Vector2Int.down) flipOffsetImages[3].enabled = true;
		}
	}
}