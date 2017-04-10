using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

[System.SerializableAttribute]
public class NoteEvent:IComparable{
	public NoteEventType eventType;
	public float beatStamp;
	public NoteMessage message;
	public NoteEvent(float beatStamp,NoteEventType type,NoteMessage message){
		this.beatStamp = beatStamp;
		this.eventType = type;
		this.message = message;
	}
    public int CompareTo(object obj)
    {
        return ((IComparable)beatStamp).CompareTo(obj);
    }
}
public class NoteMessage{
	public NoteMessage(int pitch)
	{
		this.pitch = pitch;
	}
	public NoteMessage(int pitch,float velocity)
	{
		this.pitch = pitch;
		this.velocity = velocity;
	}
	public int pitch;
	public float velocity;
}

public enum NoteEventType{NoteOn,NoteOff};