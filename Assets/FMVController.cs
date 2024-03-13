using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class FMVController : MonoBehaviour
{
	public void Play()
	{
		GetComponent<VideoPlayer>().Play();
	}
}
