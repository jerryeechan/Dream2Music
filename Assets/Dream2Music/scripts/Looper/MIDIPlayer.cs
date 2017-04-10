using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MidiJack;
using UnityEngine.UI;
public class MIDIPlayer : Singleton<MIDIPlayer> {

	[SerializeField]
	public AudioChannel[] channels;

	public List<AudioTrack> tracks;
	int currentTrackIndex = 0;
	public List<TrackData> gameSheets;
/*
	void Awake()
	{
		for(int i=0;i<channels.Length;i++)
		channels[i].channelNum = i;
	}*/
	
	AudioTrack playingTrack{
		get{
			if(tracks.Count>currentTrackIndex)
				return tracks[currentTrackIndex];
			else
				return null;
		}
	}
	AudioTrack metronomeTrack;
	bool isRecording = true;
	
	Mode mode = Mode.Compose;
	
	
	// Use this for initialization
	void Awake () {
		MidiDriver.Instance.noteOnDelegate += NoteOn;
		MidiDriver.Instance.noteOffDelegate += NoteOff;
		tracks = new List<AudioTrack>();
		prepareMetronomeTrack();
		tracks.Add(new AudioTrack(new TrackData(0)));
		tracks.Add(new AudioTrack(new TrackData(1)));
		tracks.Add(new AudioTrack(new TrackData(2)));
	}
	void prepareMetronomeTrack()
	{
		metronomeTrack = new AudioTrack(new TrackData(0));
		
		for(int i=0;i<HoneyCombConstant.loopLength;i++)
		{
			metronomeTrack.addEvent(new NoteEvent(i,NoteEventType.NoteOn,new NoteMessage(7)));
			metronomeTrack.addEvent(new NoteEvent(i+0.1f,NoteEventType.NoteOff,new NoteMessage(7)));
		}
	}
	public void createNewTrack()
	{
		tracks.Add(new AudioTrack(currentTrackIndex+1));
		currentTrackIndex++;
	}
	public void addTrack(AudioTrack track)
	{
		tracks.Add(track);
	}
	[SerializeField]
	BeatNoteGenerator gameNoteGenerator;
	[SerializeField]
	GameManager gameManager;
	
	
	void NoteOn(MidiChannel channel, int note, float velocity){
		switch(mode)
		{
			case Mode.Compose:
				if(note==63)
				{
					playingTrack.Clear();
				}
				else
				{
					if(isRecording)
					{
						//print("record on");
						playingTrack.NoteOn(new NoteMessage(note,velocity));
						playingTrack.addEvent(new NoteEvent(AudioLooper.instance.beatStamp,NoteEventType.NoteOn,new NoteMessage(note,velocity)));
					}
				}
			break;

			case Mode.PlayGame:
				if(note==63)
				{
					NextTrack();
					//gameNoteGenerator.generatePreditedNotes(playingTrack.sortedEvents);
				}
				else
				{
					playingTrack.NoteOn(new NoteMessage(note,velocity));
					gameManager.testHit(note-60);
					//playingTrack.addEvent(new NoteEvent(AudioLooper.instance.beatStamp,NoteEventType.NoteOn,new NoteMessage(note,velocity)));
				}
			break;
			case Mode.FreeStyle:
			if(note==63)
				{
					//FreeStyle play`
				}
				else
				{
					playingTrack.NoteOn(new NoteMessage(note,velocity));
				}
			break;
		}
	}
	
	void NoteOff(MidiChannel channel, int note){
		if(note==63)
		{
			return;
		}
		//playingTrack.NoteOn(new NoteMessage(note));
		if(isRecording)
		{
			print("record off");
			playingTrack.addEvent(new NoteEvent(AudioLooper.instance.beatStamp,NoteEventType.NoteOff,new NoteMessage(note)));
		}
		//channels[(int)channel].NoteOff(new NoteMessage(note));
	}
	// Update is called once per frame
	public void loopUpdate(float beatStamp) {
		//metronomeTrack.looperUpdate(beatStamp);
		if(beatStamp==0)
		{
			gameNoteGenerator.backToHead();
			metronomeTrack.backToHead();
			foreach(var track in tracks)
				track.backToHead();
		
		}
			
		gameNoteGenerator.loopUpdate(beatStamp);
		metronomeTrack.looperUpdate(beatStamp);		
		foreach(var track in tracks)
			track.looperUpdate(beatStamp);
		
		if(mode == Mode.PlayGame)
			gameManager.loopUpadate(beatStamp);
	}

	public void PlayGame()
	{
		if(mode==Mode.PlayGame)
			return;
		print("playgame");
		mode = Mode.PlayGame;
		currentTrackIndex = 0;
		//copy track data from audiotracks
		gameSheets = new List<TrackData>();
		foreach(var track in tracks)
		{
			track.backToHead();
			gameSheets.Add(track.trackData);
			//empty tracks notes
			track.Clear();
		}
		gameNoteGenerator.generatePreditedNotes(gameSheets[currentTrackIndex]);
	}	
	
	public void PreviousTrack()
	{
		if(currentTrackIndex>0)
		{
			if(mode==Mode.PlayGame)
			{
				playingTrack.trackData = gameSheets[currentTrackIndex];
				tracks[currentTrackIndex].Clear();
			}	
			currentTrackIndex--;
		}
	}
	public void NextTrack()
	{
		if(currentTrackIndex+1<tracks.Count)
		{
			if(mode==Mode.PlayGame)
			{
				playingTrack.trackData = gameSheets[currentTrackIndex];
			}
			currentTrackIndex++;
			if(mode == Mode.PlayGame)
			{
				if(gameSheets!=null)
				gameNoteGenerator.generatePreditedNotes(gameSheets[currentTrackIndex]);
			}	
		}
	}
	public void playTrack(int index)
	{
		currentTrackIndex = index;
	}
	public void bpmChanged(Slider slider)
	{
		
	}
	public void DoLoop(Toggle toggle)
	{
		if(toggle.isOn)
		{
			mode = Mode.Compose;
		}
		else{
			mode = Mode.FreeStyle;
			playingTrack.Clear();
		}
	}
}

enum Mode{
	PlayGame,Compose,FreeStyle
}