using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatNoteGenerator : Singleton<BeatNoteGenerator> {

	[SerializeField]
	BeatNoteRenderer noteRendererTemplate;
	[SerializeField]
	Transform[] padTransforms;

	//generate before the hitting timing
	
	//public static float beatNoteMoveDistance;
	//get from other thing
	
	//SortedList<float,NoteEvent> predictedNoteEvents;
	SortedList<float,BeatNoteRenderer> noteRenderers;
	
	int pitchShift = 60;

	//generate game notes hints
	public void generatePreditedNotes(TrackData trackData){
		initHintNodeDict();
		NoteEvent[] noteEvents = trackData.noteEvents;
		noteRenderers = new SortedList<float,BeatNoteRenderer>(new DuplicateKeyComparer<float>());
		foreach(var note in noteEvents)
		{
			if(note.eventType==NoteEventType.NoteOn)
			{
				var noteRenderer = Instantiate(noteRendererTemplate);
				
				var goStamp = predictNoteBeatStamp(note.beatStamp);
				noteRenderer.init(note.message.pitch,goStamp,note.beatStamp);
				noteRenderers.Add(goStamp,noteRenderer);
				

				noteRenderer.hitPostition = padTransforms[note.message.pitch-pitchShift].position;
			}
		}
	}
	float predictNoteBeatStamp(float stampOri)
	{
		float startStamp = stampOri - HoneyCombConstant.noteMoveBeatLength;
		if(startStamp<0)
		{
			startStamp += HoneyCombConstant.loopLength;
		}
		print(startStamp);
		return startStamp;
	}

	BeatNoteRenderer currentNote{
		get{
			if(noteRenderers!=null&&noteRenderers.Count>currentNoteIndex)
				return noteRenderers.Values[currentNoteIndex];
			else
				return null;
		}
	}
	int currentNoteIndex = 0;
	public void backToHead()
	{
		currentNoteIndex = 0;
	}
	public void loopUpdate(float beatStamp)
	{
		
		while(currentNote!=null&&this.ApproxmatelyTest(currentNote.goStamp,beatStamp,HoneyCombConstant.playNoteApproximation))
		{
			createNoteRenderer();
		}
		//maybe not nessersory
		/*
		foreach(var note in noteRenderers.Values)
		{
			if(note.isActive)
			{
				note.moveUpdate();
			}
		}*/
	}
	public Dictionary<int,Queue<BeatNoteRenderer>>  hintNotesDict;
	void initHintNodeDict()
	{
		hintNotesDict = new Dictionary<int,Queue<BeatNoteRenderer>>();
		for(int i=0;i<7;i++)
			hintNotesDict.Add(i,new Queue<BeatNoteRenderer>());
	}
	void createNoteRenderer()
	{
		var newNote =  Instantiate(currentNote);
		if(newNote.hitStamp!=currentNote.hitStamp)
		{
			Debug.LogError("diff stamps...");
		}
//		print(newNote.goStamp);
//		print("hit stamp"+newNote.hitStamp);
		//newNote.bindEvent = currentNote.bindEvent;
		newNote.gameObject.SetActive(true);
		newNote.Go();
		var hintNotes = hintNotesDict[newNote.pitch-pitchShift];
		hintNotes.Enqueue(newNote);
		newNote.bindQueue = hintNotes;
		currentNoteIndex++;
	}


}
