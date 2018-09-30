using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public AudioSource EffectsSource;
	public AudioSource MusicSource;
	public static AudioManager Instance = null;
	
	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance != this)
		{
			Destroy(gameObject);
		}

		DontDestroyOnLoad (gameObject);
	}

	public void Play(AudioClip clip, float volume)
	{
		EffectsSource.clip = clip;
        EffectsSource.volume = volume;
		EffectsSource.Play();
	}

	public void PlayMusic(AudioClip clip, float volume)
	{
		MusicSource.clip = clip;
        MusicSource.volume = volume;
		MusicSource.Play();
	}
}