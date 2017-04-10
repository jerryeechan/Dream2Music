using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.SerializableAttribute]
//for game sheet
public class TrackData{
	public TrackData(int channel)
	{
		this.channel = channel;
	}
	public int channel;
	public NoteEvent[] noteEvents;
}
