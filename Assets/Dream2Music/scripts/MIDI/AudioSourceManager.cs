using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceManager : MonoBehaviour{
	public List<AudioSource> sources;
	int sourceNum = 7;
	void Awake()
	{
		sources = new List<AudioSource>();
		for(int i=0;i<sourceNum;i++)
		{
			sources.Add(gameObject.AddComponent<AudioSource>());
			//sources[i].outputAudioMixerGroup
			
		}
	}
	
	public void PlayOneShot(AudioClip clip,float volume)
	{
		var source = sources[0];
		source.PlayOneShot(clip,volume);
		//sources.RemoveAt(0);
	}
	public AudioSource PlaySound(AudioClip clip)
	{
		var source = sources[0];
		source.clip = clip;
		source.Play();
		sources.RemoveAt(0);
		return source;
	}
	public void StopSound(AudioSource source)
	{
		source.Stop();
		sources.Add(source);
	}
}
