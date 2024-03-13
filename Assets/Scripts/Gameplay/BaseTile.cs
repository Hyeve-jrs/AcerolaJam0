using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseTile : Tile
{
	public AnimationCurve spawnAnimCurve;
	public AnimationCurve removedAnimCurve;
	public AnimationCurve flipAnimCurve;
	public float spawnAnimTime;
	public float removedAnimTime;
	public float flipAnimTime;
	public int x, y;
	public GridController owner;

	protected bool isAnimating;

	public override GameObject GetAssociated()
	{
		return gameObject;
	}

	public override void Assign(int x, int y, GridController controller)
	{
		this.x = x;
		this.y = y;
		owner = controller;
	}

	public override void Clicked()
	{
		if(!owner.CanActionsHappen()) return;
		OnClicked();
	}

	public override void Interacted(Tile by)
	{
		OnInteracted(by);
	}

	protected abstract void OnClicked();
	protected abstract void OnInteracted(Tile by);

	public override bool AllAnimsFinished()
	{
		return !isAnimating;
	}

	public override void Remove()
	{
		StartCoroutine(RemoveRoutine());
	}

	protected virtual void Start()
	{
		StartCoroutine(SpawnRoutine());
	}

	private IEnumerator SpawnRoutine()
	{
		isAnimating = true;
		transform.localScale = Vector3.zero;
		
		float timer = 0f;
		float delay = (x + y) * 0.1f;
		yield return new WaitForSeconds(delay);
		
		while (timer < spawnAnimTime)
		{
			timer += Time.deltaTime;
			float alpha = timer / spawnAnimTime;
			float scale = spawnAnimCurve.Evaluate(alpha);
			transform.localScale = new Vector3(scale, scale, 1);
			yield return null;
		}
		
		transform.localScale = Vector3.one;
		isAnimating = false;
	}
	
	private IEnumerator RemoveRoutine()
	{
		isAnimating = true;
		transform.localScale = Vector3.one;
		
		float timer = 0f;
		float delay = (x + y) * 0.1f;
		yield return new WaitForSeconds(delay);
		
		while (timer < removedAnimTime)
		{
			timer += Time.deltaTime;
			float alpha = timer / removedAnimTime;
			float scale = 1.0f - removedAnimCurve.Evaluate(alpha);
			transform.localScale = new Vector3(scale, scale, 1);
			yield return null;
		}
		
		transform.localScale = Vector3.zero;
		isAnimating = false;
	}
}