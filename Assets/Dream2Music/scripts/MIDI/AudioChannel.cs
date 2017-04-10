using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent (typeof (AudioSourceManager))]
public class AudioChannel : MonoBehaviour {
	public int channelNum;

	[SerializeField]
	AudioSource masterAudioSource;
	[SerializeField]
	AudioClip[] clips;
	AudioSourceManager audioPool;
	void Awake(){
		audioPool = GetComponent<AudioSourceManager>();
		playingSource = new Dictionary<int,AudioSource>();
	}

	Dictionary<int,AudioSource> playingSource;
	void Start()
	{
		foreach(var source in audioPool.sources)
		{
			source.outputAudioMixerGroup = masterAudioSource.outputAudioMixerGroup;
		}	
	}
	public float volume = 1;
	public void NoteOn(NoteMessage message)
	{	
		audioPool.PlayOneShot(GetClip(message.pitch),volume);
		/*
		var source = audioPool.PlaySound(GetClip(message.pitch));
		if(playingSource.ContainsKey(message.pitch))
		{
			playingSource[message.pitch] = source;
		}
		else
		{
			playingSource.Add(message.pitch,source);
		}*/
	}
	public void NoteOff(NoteMessage message)
	{	
		if(playingSource.ContainsKey(message.pitch))
		{
			audioPool.StopSound(playingSource[message.pitch]);
			playingSource[message.pitch] = null;
		}
	}
	public AudioClip GetClip(int pitch)
	{
		if(pitch>=60)
			pitch -= 60;
		if(pitch < clips.Length)
			return clips[pitch];
		else
		{
			Debug.LogError("out of bound: "+pitch);
			return clips[0];
		}
	}
}
