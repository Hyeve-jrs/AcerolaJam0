using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
[RequireComponent(typeof(RawImage))]
public class FMVRenderToCanvas : MonoBehaviour
{
	public float AlphaIn;
	public float AlphaHold;
	public float AlphaOut;

	public float InTime;
	public float OutTime;
	
	private RenderTexture texture;
	private VideoPlayer player;
	private RawImage image;
	
    void Start()
    {
	    player = GetComponent<VideoPlayer>();
        texture = RenderTexture.GetTemporary((int)player.width, (int)player.height);
        player.renderMode = VideoRenderMode.RenderTexture;
        player.targetTexture = texture; 
        image = GetComponent<RawImage>();
	    image.texture = texture;
	    image.color = Color.white;
    }
    
    void Update()
    {
	    if (!player.isPrepared)
	    {
		    player.time = 0f;
		    return;
	    }
	    
	    
	    if (player.time < InTime)
	    {
		    float aIn = (float)(player.time / InTime);
		    Color col = new(1f, 1f, 1f, Mathf.Lerp(AlphaIn, AlphaHold, aIn));
		    image.color = col;
		    return;
	    }
	    
	    if (player.length - player.time < OutTime)
	    {
		    float aOut = (float)((player.length - player.time) / OutTime);
		    Color col = new(1f, 1f, 1f, Mathf.Lerp(AlphaOut, AlphaHold, aOut));
		    image.color = col;
		    return;
	    }

	    image.color = new Color(1f, 1f, 1f, AlphaHold);
    }

    private void OnDestroy()
    {
	    RenderTexture.ReleaseTemporary(texture);
    }
}
