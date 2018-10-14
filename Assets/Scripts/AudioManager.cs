using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    private AudioSource source;
    public Sound[] sounds;
	
    private void Awake()
    {
        // Setup the global instance.
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // There can only be one audio manager
        }

        source = GetComponent<AudioSource>();
    }

	public void Play(string name)
	{
        if (source.isPlaying)
        {
            Stop(); // Stop the current clip if it's running
        }

        foreach (Sound sound in sounds)
        {
            if (sound.name == name)
            {
                // Play the sound.
                source.loop = sound.loop;
                source.volume = sound.volume;
                source.clip = sound.source;
                source.Play();
                return;
            }
        }
	}

    public void Stop()
    {
        source.Stop();
    }

    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip source;
        [Range(0, 1)]
        public float volume;
        public bool loop;
    }
}