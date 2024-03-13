using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SfxType
{
	Swoosh,
	TextClick,
	TileFLip,
}

public enum MusicType
{
	Type1,
	Type2,
	Type3,
}

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
	public AudioClip SwooshClip;
	public AudioClip TextClickClip;
	public AudioClip TileFLipClip;

	public AudioClip Music1;
	public AudioClip Music2;
	public AudioClip Music3;

	private AudioSource source;
	private float volMultiplier = 1.0f;
	private float internalVol = 1.0f;
	private static AudioManager INSTANCE;
    void Start()
    {
	    if (INSTANCE != null)
	    {
		    Destroy(gameObject);
		    return;
	    }
	    
	    
        DontDestroyOnLoad(gameObject);
        source = GetComponent<AudioSource>();
        INSTANCE = this;
    }

    private void PlayInternal(SfxType type)
    {
	    switch (type)
	    {
		    case SfxType.Swoosh:
			    source.PlayOneShot(SwooshClip);
			    break;
		    case SfxType.TextClick:
			    source.PlayOneShot(TextClickClip);
			    break;
		    case SfxType.TileFLip:
			    source.PlayOneShot(TileFLipClip);
			    break;
	    }
    }

    private void SwitchMusicInternal(MusicType type)
    {
	    StopAllCoroutines();
	    StartCoroutine(MusicFadeSwapCoroutine(type));
    }

    private void Update()
    {
	    source.volume = internalVol * volMultiplier;
    }

    private IEnumerator MusicFadeSwapCoroutine(MusicType type)
    {
	    while (internalVol > 0.0)
	    {
		    internalVol -= Time.deltaTime;
		    yield return null;
	    }

	    switch (type)
	    {
		    case MusicType.Type1:
			    source.clip = Music1;
			    break;
		    case MusicType.Type2:
			    source.clip = Music2;
			    break;
		    case MusicType.Type3:
			    source.clip = Music3;
			    break;
	    }
	    
	    while (internalVol < 1.0)
	    {
		    internalVol += Time.deltaTime;
		    yield return null;
	    }
    }
    
    public static void SwitchMusic(MusicType type)
    {
	    if(INSTANCE != null) INSTANCE.SwitchMusicInternal(type);
    }

    public static void PlaySfx(SfxType type)
    {
	    if(INSTANCE != null) INSTANCE.PlayInternal(type);
    }

    public static void SetVolume(float vol)
    {
	    if (INSTANCE != null)
	    {
		    INSTANCE.volMultiplier = vol;
	    }
    }
}
