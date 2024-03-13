#if UNITY_EDITOR 
using UnityEditor;
#endif
using UnityEngine;

public class QuitScript : MonoBehaviour
{
	public void Quit()
	{
		if (Application.isEditor)
		{
			#if UNITY_EDITOR 
			EditorApplication.ExitPlaymode();
			#endif
		}
		else
		{
			Application.Quit();
		}
	}
}
