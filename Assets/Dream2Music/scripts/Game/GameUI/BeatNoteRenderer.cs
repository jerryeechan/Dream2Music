using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BeatNoteRenderer : MonoBehaviour {
	public int pitch;
	public float goStamp;
	//public NoteEvent bindEvent;
	public float hitStamp;
	public Queue<BeatNoteRenderer> bindQueue;
	public void init(int pitch,float goTime,float hitTime)
	{
		goStamp = goTime;
		this.pitch = pitch;
		this.hitStamp = hitTime;
	}
	SpriteRenderer spr;
	[HideInInspector]
	public Vector3 hitPostition;
	public bool isHittable;

	void Awake()
	{
		spr = GetComponent<SpriteRenderer>();
	}
	public bool isActive
	{
		get{
			return gameObject.activeSelf;
		}
	}
	
	public void Go(){
		var timeLength = (float)60/HoneyCombConstant.bpm*(HoneyCombConstant.noteMoveBeatLength);
		print(timeLength);
		isHittable = true;
		transform.localScale = Vector3.one*5;
		transform.position = (hitPostition)*6;
		transform.DOMove(hitPostition,timeLength);
		transform.DOScale(Vector3.one,timeLength).OnComplete(()=>{
			this.myInvoke(0.1f,missAnimation);
		});
	}
	
	public void hitAnimation(int score)
	{
		
		if(isHittable)
		{
			UIManager.instance.ShowHitScore(score);
			hit = true;
			isHittable = false;
			transform.DOScale(2,0.5f);
			var color = Color.yellow;
			if(score == 2)
			color = Color.cyan;
			spr.DOColor(color,0.2f).OnComplete(()=>{
				Done();
			});
		}
	}
	public void foolAnimation()
	{
		if(isHittable)
		{
			UIManager.instance.ShowHitScore(-1);
			isHittable = false;
			//transform.DOScale(2,0.5f);
			spr.DOColor(Color.red,0.5f).OnComplete(()=>{
				Done();
			});
		}
	}
	public void missAnimation()
	{
		
		if(isHittable)
		{
			UIManager.instance.ShowHitScore(-1);
			isHittable = false;
			spr.color = Color.black;
			spr.DOColor(Color.clear,0.5f).OnComplete(()=>{
				Done();
			});
		}
		
	}
	bool hit = false;
	public void Done()
	{
		if(!hit)
			bindQueue.Dequeue();
		Destroy(gameObject);
	}
}