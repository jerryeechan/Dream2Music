using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class UIManager : Singleton<UIManager>{

	public Text hitScoreText;
	public void ShowHitScore(int score)
	{
		if(score==3)
			hitScoreText.text = "Perfect";
		else if(score==2)
			hitScoreText.text = "Good";
		else if(score==-1)
			hitScoreText.text = "Foolish";
		hitScoreText.DOKill();
		hitScoreText.color = Color.white;
		hitScoreText.transform.localScale = Vector3.zero;
		hitScoreText.transform.DOScale(Vector3.one,0.2f).OnComplete(()=>{
			hitScoreText.DOColor(Color.clear,1f);
		});
	}
	
}
