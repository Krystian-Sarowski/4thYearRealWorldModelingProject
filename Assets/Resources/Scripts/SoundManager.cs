using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
	public static SoundManager s_instance;

	public AudioMixerGroup mixerGroupSFX;
	public AudioMixerGroup mixerGroupMusic;

	public Sound[] m_soundEffects;
	public Sound[] m_musicTracks;

	public static float s_sfxVolume;
	public static float s_musicVolume;

	void Awake()
	{
		//Makes sure there is only one audio manager
		if (s_instance != null)
		{
			DestroyImmediate(gameObject);
			return;
		}
		else
		{
			s_instance = this;
			s_musicVolume = 0.5f;
			s_sfxVolume = 0.5f;
			DontDestroyOnLoad(gameObject);
		}

		//Makes a new sound object and sets its varables
		foreach (Sound s in m_soundEffects)
		{
			s.m_source = gameObject.AddComponent<AudioSource>();
			s.m_source.clip = s.m_clip;
			s.m_source.loop = s.m_loop;

			s.m_source.outputAudioMixerGroup = mixerGroupSFX;
		}

		foreach (Sound music in m_musicTracks)
		{
			music.m_source = gameObject.AddComponent<AudioSource>();
			music.m_source.clip = music.m_clip;
			music.m_source.loop = music.m_loop;
			music.m_source.priority = 50;

			music.m_source.outputAudioMixerGroup = mixerGroupMusic;
		}
	}

	/// <summary>
	/// Plays the sound based on the string that is passed in, it searches for the sound name with the same name as the string
	/// </summary>
	/// <param name="t_soundName">name of the sound</param> 
	public void Play(string t_soundName)
	{
		Sound s = Array.Find(m_soundEffects, item => item.m_name == t_soundName);
		
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		s.m_source.volume = s.m_volume;
		s.m_source.pitch = s.m_pitch;

		s.m_source.Play();
	}

	public void PlayMusic(string t_musicName)
    {
		Sound musicSound = Array.Find(m_musicTracks, item => item.m_name == t_musicName);

		if (musicSound == null)
		{
			Debug.LogWarning("Music: " + name + " not found!");
			return;
		}

		foreach (Sound music in m_musicTracks)
		{
			if (music.m_source.isPlaying)
			{
				if(music.m_name != t_musicName)
                {
					music.m_source.Stop();
				}
			}
		}

		if(!musicSound.m_source.isPlaying)
        {
			musicSound.m_source.volume = musicSound.m_volume;
			musicSound.m_source.pitch = musicSound.m_pitch;

			musicSound.m_source.Play();
		}
	}

	public void PlayRandomBackgroundTrack()
    {
		Sound[] backgroundTracks = Array.FindAll(m_musicTracks, item => item.m_name.Contains("Background"));

		int random = UnityEngine.Random.Range(0, backgroundTracks.Length);

		PlayMusic(backgroundTracks[random].m_name);
	}

	public void Stop(string t_soundName)
    {
		Sound s = Array.Find(m_soundEffects, item => item.m_name == t_soundName);

		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		s.m_source.Stop();
	}

	public void PauseAllSoundEffects()
    {
		foreach (Sound sound in m_soundEffects)
		{
			if (sound.m_source.isPlaying)
			{
				sound.m_source.Pause();
			}
		}
	}

	public void UnpauseAllSoundEffects()
	{
		foreach (Sound sound in m_soundEffects)
		{
			sound.m_source.UnPause();
		}
	}

	public void StopAllSounds()
    {
		foreach(Sound sound in m_soundEffects)
        {
			if(sound.m_source.isPlaying)
            {
				sound.m_source.Stop();
            }
        }
    }
}
