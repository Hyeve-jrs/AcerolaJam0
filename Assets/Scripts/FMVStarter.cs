using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class FMVStarter : MonoBehaviour
{
	public float Delay;

	private float timer;
	
    void Update()
    {
	    timer += Time.deltaTime;
	    if (timer > Delay)
	    {
		    GetComponent<VideoPlayer>().Play();
		    Destroy(this);
	    }
    }
}
