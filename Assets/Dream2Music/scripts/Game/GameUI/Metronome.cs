using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Metronome : MonoBehaviour {

	SpriteRenderer spr;
	
	void Awake()
	{
		spr = GetComponent<SpriteRenderer>();
		setTempo(120);
		
	}
	public float scale;
	public void setTempo(int bpm)
	{
		float spb = SecondPerBeat(bpm);
		transform.DOKill();
		transform.DOScale(scale,spb/2).SetLoops(-1,LoopType.Yoyo).SetEase(Ease.InOutSine);
	}
	void done()
	{
		
	}
	public static float SecondPerBeat(int bpm)
	{
		return 1/((float)bpm/60);
	}
}
