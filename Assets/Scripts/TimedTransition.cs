using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimedTransition : MonoBehaviour
{
	public string SceneTo;
	public float DelayTime;
	public bool StartLoadImmediately;

	private float timer;
	private bool loading;

	private void Update()
	{
		if (loading) return;
		timer += Time.deltaTime;
		if (timer > DelayTime || StartLoadImmediately) StartCoroutine(LoadRoutine());
	}

	private IEnumerator LoadRoutine()
	{
		loading = true;
		AsyncOperation op = SceneManager.LoadSceneAsync(SceneTo);
		op.allowSceneActivation = false;
		while (timer < DelayTime || op.progress < 0.9f)
		{
			timer += Time.deltaTime;
			yield return null;
		}
		op.allowSceneActivation = true;
	}
}
