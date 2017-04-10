using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[SerializableAttribute]
public class AudioTrack{
    public string name;
    public bool mute=false;
    public AudioTrack(TrackData data)
    {
        trackData = data;
    }
    public AudioTrack(int channel)
    {
        bindChannel = MIDIPlayer.instance.channels[channel];
        sortedEvents = new SortedList<float,NoteEvent>(new DuplicateKeyComparer<float>());
        freshNotes = new List<NoteEvent>();
    }
    
    public AudioChannel bindChannel;

    TrackData _trackData;
    public TrackData trackData{
        get{
            _trackData.noteEvents = new NoteEvent[sortedEvents.Count];
            Debug.Log(bindChannel.channelNum);
            _trackData.channel = bindChannel.channelNum;
            sortedEvents.Values.CopyTo(_trackData.noteEvents,0);
            return _trackData;
        }
        set{
            _trackData = value;
            bindChannel = MIDIPlayer.instance.channels[value.channel];
            sortedEvents = new SortedList<float,NoteEvent>(new DuplicateKeyComparer<float>());
            freshNotes = new List<NoteEvent>();
            if(value.noteEvents!=null)
            {
                foreach(var eve in value.noteEvents)
                {
                    sortedEvents.Add(eve.beatStamp,eve);
                }
            }
        }
    }

    //temp events;
    //List<NoteEvent> noteEvents;
    
    public SortedList<float,NoteEvent> sortedEvents;
    
  
    int currentEventIndex = 0;
    public void backToHead()
    {
        currentEventIndex = 0;
        foreach(var ne in freshNotes)
            sortedEvents.Add(ne.beatStamp,ne);
        freshNotes.Clear();
    }
    public void looperUpdate(float beatstamp)
    {
        while(testEvent(beatstamp)){}
    }
    public bool testEvent(float beatstamp)
    {   
        if(sortedEvents.Count<=currentEventIndex)
        {
            return false;
        }
        else{
            var currentEvent = sortedEvents.Values[currentEventIndex];
            
            if(bindChannel.ApproxmatelyTest(currentEvent.beatStamp,beatstamp,HoneyCombConstant.playNoteApproximation))
            {
//                Debug.Log(""+beatstamp+" "+currentEvent.beatStamp);
                triggerEvent(currentEvent);
                currentEventIndex++;
                return true;
            }
            return false;
        }
    }
    //Note[] notes;
    public void triggerEvent(NoteEvent ne)
    {
        switch(ne.eventType)
        {
            case NoteEventType.NoteOn:
                NoteOn(ne.message);
            break;
            case NoteEventType.NoteOff:
                NoteOff(ne.message);
                //notes[ne.message.pitch].Stop();
            break;
        }
    }
    List<NoteEvent> freshNotes;
    public void addEvent(NoteEvent ne)
    {
        //noteEvents.Add(ne);
        freshNotes.Add(ne);
        
    }
    public void NoteOn(NoteMessage message)
    {
        if(mute==false)
            bindChannel.NoteOn(message);
    }
    public void NoteOff(NoteMessage message)
    {
        bindChannel.NoteOff(message);
    }
    public void Clear()
    {
        sortedEvents.Clear();
        freshNotes.Clear();
    }
}
public class DuplicateKeyComparer<TKey>:IComparer<TKey> where TKey : IComparable
{
    #region IComparer<TKey> Members

    public int Compare(TKey x, TKey y)
    {
        int result = x.CompareTo(y);

        if (result == 0)
            return 1;   // Handle equality as beeing greater
        else
            return result;
    }

    #endregion
}