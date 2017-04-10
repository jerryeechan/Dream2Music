using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	//AudioTrack playingTrack;
	
	//const float goodTimeDis = 0.1f;


	
	Queue<int> pressedNotePitchQueue;
	void Awake()
	{
		pressedNotePitchQueue = new Queue<int>();
	}
	public void testHit(int pitch)
	{
		pressedNotePitchQueue.Enqueue(pitch);
	}
	float lastStamp = 0;
	public void loopUpadate(float beatStamp)
	{
		lastStamp = beatStamp;
		while(pressedNotePitchQueue.Count>0)
		{
			var pitch = pressedNotePitchQueue.Dequeue();			
			var queue = BeatNoteGenerator.instance.hintNotesDict[pitch];
			if(queue.Count==0)
				continue;
			var closestNote = queue.Peek();
			print("diff"+(closestNote.hitStamp-beatStamp)+"current:"+beatStamp+"hitstamp"+closestNote.hitStamp);
			if(!closestNote.isHittable)
				continue;

			if(this.ApproxmatelyTest(closestNote.hitStamp,beatStamp,HoneyCombConstant.perfectTimeDis))
			{
				print("hit 3 ");
				queue.Dequeue();
				closestNote.hitAnimation(3);
			}
			else if(this.ApproxmatelyTest(closestNote.hitStamp,beatStamp,HoneyCombConstant.goodTimeDis))
			{
				print("hit 2");
				queue.Dequeue();
				closestNote.hitAnimation(2);
			}
			else{
				//foolish~
				
				closestNote.foolAnimation();
			}
		}
	}
}
