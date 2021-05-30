using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
//Sound variable that will be used in the sound manager
public class Sound
{
	public string m_name;					//Name of the sound

	public AudioClip m_clip;				//The audio clip of the sound

	[Range(0f, 1f)]						
	public float m_volume = .75f;			//Volume parameter that can be changed in the inspector

	[Range(.1f, 3f)]					
	public float m_pitch = 1f;				//Pitch of the sound effect, this can also be changed in the inspector

	public bool m_loop = false;				//Sets loop to false as its a sound effect rather than a background music

	[HideInInspector]
	public AudioSource m_source;
}
