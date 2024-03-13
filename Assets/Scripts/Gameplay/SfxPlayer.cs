using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxPlayer : MonoBehaviour
{
	public SfxType type;

	public void Play()
	{
		AudioManager.PlaySfx(type);
	}
}
