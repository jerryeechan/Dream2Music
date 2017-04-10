using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class AudioLooper :Singleton<AudioLooper>{

	//beats

	
	
	public SongProperty generateSongData()
	{
		return new SongProperty(HoneyCombConstant.loopLength,HoneyCombConstant.bpm);
	}
	public void LoadSongData()
	{
		var songData = DataManager.instance.songData[0];
		var trackDataArray = songData.trackData;
		foreach(var trackData in trackDataArray)
		{
			player.addTrack(new AudioTrack(trackData));
		}
	}
	public float beatStamp
	{
		get{
			return currentStamp;
		}
	}
	public float currentStamp;
	float step;
	[SerializeField]
	float stepTime;
	void Awake()
	{
		circularPath = new Vector3[128];
		float radius = 2;
		float start = -Mathf.PI;
		float end = Mathf.PI;
		float step = (end - start)/128;
		int i=0;
		for(float d=start;i<128;d+=step,i++)
		{
			circularPath[i] = new Vector3(Mathf.Cos(d),Mathf.Sin(d))*radius;
		}
	}
	Vector3[] circularPath;
	void Start()
	{
		player = MIDIPlayer.instance;
		startPlay();
	}
	public void Play()
	{
		startPlay();
	}
	void startPlay()
	{
		step = (float)1/HoneyCombConstant.loopLength; //1/(bpm/60)/8;
		stepTime = step*1/(HoneyCombConstant.bpm/60);
		transform.position = circularPath[0];
		var duration = HoneyCombConstant.loopLength*Metronome.SecondPerBeat(HoneyCombConstant.bpm);
		var t = transform.DOPath(circularPath,duration).SetLoops(-1).SetEase(Ease.Linear).OnUpdate(looperUpdate);
		
		//60bpm 1beat 1sec
	}
	MIDIPlayer player;
	void looperUpdate()
	{
		//print(currentStamp);
		/*
		foreach(var channel in channels)
		{
			channel.tryNote(currentStamp);
		}*/
		
		player.loopUpdate(currentStamp);
		currentStamp+=Time.deltaTime*HoneyCombConstant.bpm/60;
		if(currentStamp>=HoneyCombConstant.loopLength)
		{
			currentStamp = 0;
		}
	}
}

public class Instrument
{
	[SerializeField]
	AudioClip[] clips;
	int index;
	public AudioClip currentClip{
		get{
			return clips[index];
		}
	}
}
