using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.SerializableAttribute]
public class SongData {
	
	public TrackData[] trackData;
	
	
}


[System.SerializableAttribute]
public class SongProperty{
	public SongProperty(int length,int bpm)
	{
		this.loopLength = length;
		this.bpm = bpm;
	}
	public int loopLength;
	public int bpm;
}