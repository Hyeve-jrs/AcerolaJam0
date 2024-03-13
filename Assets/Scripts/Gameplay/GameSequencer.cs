using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class SequenceStep
{
	public Level level;
	public string hintText;


	private bool levelDone;
	private GridController controller;
	
	public void Start(GridController grid)
	{
		controller = grid;
		controller.onLevelWon.AddListener(LevelDone);
		controller.StartLevel(level);
		controller.SetHintText(hintText);
	}

	public void Update()
	{
		
	}

	public void End() {
		controller.onLevelWon.RemoveListener(LevelDone);	
	}

	public bool IsDone()
	{
		return levelDone;
	}
	
	private void LevelDone()
	{
		levelDone = true;
	}
}

public class GameSequencer : MonoBehaviour
{
	public GridController controller;
	public bool reset;
	public bool allowWin = true;

	public UnityEvent onFinished;
	
	[SerializeField]
	public SequenceStep[] sequence;
	
	private int index = 0;
	private SequenceStep currentAction;

	private void Start()
	{
		StartNextAction();
	}

	private void Update()
	{
		if (reset)
		{
			reset = false;
			RestartAction();
			return;
		}

		if (currentAction == null) return;
		currentAction.Update();
		if (!currentAction.IsDone() || !allowWin) return;
		currentAction.End();
		StartNextAction();
	}

	public void RestartAction()
	{
		currentAction.End();
		index--;
		StartNextAction();
	}

	[ContextMenu("TestFinish")]
	void TestFinish()
	{
		onFinished.Invoke();
	}
	
	public void StartNextAction()
	{
		currentAction = null;
		if (index > sequence.Length - 1)
		{
			onFinished.Invoke();
			return;
		}
		
		currentAction = sequence[index];
		currentAction.Start(controller);
		
		index++;
	}
}
