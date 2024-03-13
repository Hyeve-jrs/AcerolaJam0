using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ImageFlipper : FlipPart
{
	public Color active;
	public Color inactive;

	public Image[] images;

	public override void SetActive(bool active)
	{
		foreach (Image image in images)
		{
			image.color = active ? this.active : inactive;
		}
	}
}