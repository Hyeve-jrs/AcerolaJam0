using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FMVTransitioner : MonoBehaviour
{
	public string SceneTo;
	public float TransitionTime;
	public GameObject TransitionObject;
	
	public void StartTransition()
	{
		TransitionObject.SetActive(true);
		StartCoroutine(LoadRoutine());
	}

	private IEnumerator LoadRoutine()
	{
		AsyncOperation op = SceneManager.LoadSceneAsync(SceneTo);
		op.allowSceneActivation = false;
		float timer = 0f;
		while (timer < TransitionTime || op.progress < 0.9f)
		{
			timer += Time.deltaTime;
			yield return null;
		}
		op.allowSceneActivation = true;
	}
}
