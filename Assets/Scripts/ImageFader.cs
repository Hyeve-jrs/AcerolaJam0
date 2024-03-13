using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ImageFader : MonoBehaviour
{
	private Image image;

	public float AlphaStart;
	public float AlphaEnd;
	public float FadeTime;
	public float FadeDelay;

	private float time;
	private float delayTime;

	private Color baseCol;
	
	void Start()
	{
		image = GetComponent<Image>();
		baseCol = image.color;
		image.color = new Color(baseCol.r, baseCol.g, baseCol.b, AlphaStart);
	}

	void Update()
	{

		if (delayTime < FadeDelay)
		{
			delayTime += Time.deltaTime;
			return;
		}

		time += Time.deltaTime;
		float deltaFade = time / FadeTime;
		
		if (deltaFade < 1)
		{
			float alpha = Mathf.Lerp(AlphaStart, AlphaEnd, deltaFade);
			image.color = new Color(baseCol.r, baseCol.g, baseCol.b, alpha);
			return;
		}

		image.color = new Color(baseCol.r, baseCol.g, baseCol.b, AlphaEnd);
		Destroy(this);
	}
}
