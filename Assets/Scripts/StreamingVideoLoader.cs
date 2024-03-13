using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class StreamingVideoLoader : MonoBehaviour
{
	public string AssetFile;
	public bool playNow = true;
	private void Start()
	{
		VideoPlayer player = GetComponent<VideoPlayer>();
		string path = System.IO.Path.Combine(Application.streamingAssetsPath, AssetFile);
		player.url = path;
		if(playNow) player.Play();
	}
}
