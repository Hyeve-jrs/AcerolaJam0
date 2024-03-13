using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class FMVCanvasFader : MonoBehaviour
{
	private VideoPlayer player;

	public float AlphaIn;
	public float AlphaHold;
	public float AlphaOut;

	public float InTime;
	public float OutTime;
	
	void Start()
    {
	    player = GetComponent<VideoPlayer>();
    }

    void Update()
    {

	    if (player.time < InTime)
	    {
		    float aIn = (float)(player.time / InTime);
		    player.targetCameraAlpha = Mathf.Lerp(AlphaIn, AlphaHold, aIn);
		    return;
	    }
	    
	    if (player.length - player.time < OutTime)
	    {
		    float aOut = (float)((player.length - player.time) / OutTime);
		    player.targetCameraAlpha = Mathf.Lerp(AlphaOut, AlphaHold, aOut);
		    return;
	    }

	    player.targetCameraAlpha = AlphaHold;
    }
}
