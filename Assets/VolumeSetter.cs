using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSetter : MonoBehaviour
{
	public Slider associatedSlider; 
	
	public void OnVolumeKnobMoved()
	{
		AudioManager.SetVolume(associatedSlider.value);
	}
}
