﻿using UnityEngine.Audio;
using UnityEngine;

/* this script creates the public sounds list that will be handled by AudioManager */

[System.Serializable]
public class Sound
{

	public string name;

	public AudioClip clip;

	[Range(0f, 1f)]
	public float volume = .75f;
	[Range(0f, 1f)]
	public float volumeVariance = .1f;

	[Range(.1f, 3f)]
	public float pitch = 1f;
	[Range(0f, 1f)]
	public float pitchVariance = .1f;

	public bool loop = false;

	public AudioMixerGroup mixerGroup;

	// we want it to be accessed from AudioManager but not from inspector
	[HideInInspector]
	public AudioSource source;

}
