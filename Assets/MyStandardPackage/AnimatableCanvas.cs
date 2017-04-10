using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
[RequireComponent (typeof (CanvasGroup))]
public class AnimatableCanvas : MonoBehaviour {
    public bool isAnimating = false;
    public float duration;
    public bool isHideStart = false;
    CanvasGroup canvasGroup;
	protected virtual void Awake()
	{
        canvasGroup = GetComponentInChildren<CanvasGroup>();
        if(canvasGroup==null)
        {
            gameObject.AddComponent<CanvasGroup>();
        }
	}
    protected virtual void Start()
    {
        if(isHideStart)
        {
            hide(0);
        }
    }
    
	public virtual void hide()
    {
        hide(null);
    }
    public virtual void hide(OnCompleteDelegate completeEvent)
    {
        hide(duration,completeEvent);
    }
	public virtual void hide(float duration,OnCompleteDelegate completeEvent=null)
	{   
            if(!canvasGroup)
            {
                canvasGroup = GetComponentsInChildren<CanvasGroup>(true)[0];
            }
            
            if(canvasGroup)
            {
                canvasGroup.alpha = 1;
                canvasGroup.DOKill(true);
                canvasGroup.DOFade(0,duration).OnComplete(
                ()=>{
                    if(completeEvent!=null)
                      completeEvent();
                    gameObject.SetActive(false);
//                    print(name+"hide complete");
                });
            }
            else{
                Debug.LogError(name+"CanvasGroup not exist");
            }
	}
	
    public virtual void show()
    {
        show(null);
    }
    public virtual void show(OnCompleteDelegate completeEvent=null)
    {
        show(duration,completeEvent);
    }
	public virtual void show(float duration,OnCompleteDelegate completeEvent=null)
	{
//        print(name+" show start");
        if(canvasGroup==null)
            canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.DOKill(true);
        canvasGroup.alpha = 0;
        activate();
        if(canvasGroup)
        {
            canvasGroup.DOFade(1,duration).OnComplete(
            ()=>{
//                print(name+"show complete");
                
                if(completeEvent!=null)
                    completeEvent();
                else{
//                    print(name+"no complete event");
                }
            });
        }
        else{
            Debug.LogError("No canvas Group!");
        }
         
        
	}
    
	protected virtual void activate()
	{
		gameObject.SetActive(true);
	}
}
public delegate void OnCompleteDelegate();
