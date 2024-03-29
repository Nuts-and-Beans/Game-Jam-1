using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

	public static AudioManager i;

	public AudioMixerGroup mixerGroup;

	public  Sound[] sounds;



	void Awake()
	{
		if (i != null)
		{
			Destroy(gameObject);
		}
		else
		{
			i = this;
			DontDestroyOnLoad(gameObject);
		}

		foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.loop = s.loop;
			s.source.outputAudioMixerGroup = mixerGroup;
		}
	}

	private void PlayInstance(string sound)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
		s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

		s.source.Play();
		
	}
	

	public static void Play(string sound)
	{
		if (i == null)
		{
			Debug.LogWarning("No audio Manager");

			return;
		}

		i.PlayInstance(sound);
	}
	private void StopInstance(string sound)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		s.source.Stop();
	}

	public static void Stop(string sound)
	{
		if (i == null)
		{
			Debug.LogWarning("No audio Manager");

			return;
		}
		i.StopInstance(sound);
	}
}
