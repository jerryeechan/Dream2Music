using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
public static class Extensions
{
	public static void SetPositionX(this Transform t, float newX)
	{
		t.position = new Vector3(newX, t.position.y, t.position.z);
	}
	
	public static void SetPositionY(this Transform t, float newY)
	{
		t.position = new Vector3(t.position.x, newY, t.position.z);
	}
	
	public static void SetPositionZ(this Transform t, float newZ)
	{
		t.position = new Vector3(t.position.x, t.position.y, newZ);
	}
	
	public static float GetPositionX(this Transform t)
	{
		return t.position.x;
	}
	
	public static float GetPositionY(this Transform t)
	{
		return t.position.y;
	}
	
	public static float GetPositionZ(this Transform t)
	{
		return t.position.z;
	}
	
	public static bool HasRigidbody(this GameObject gobj)
	{
		return (gobj.GetComponent<Rigidbody>() != null);
	}
	
	public static bool HasAnimation(this GameObject gobj)
	{
		return (gobj.GetComponent<Animation>() != null);
	}
	
	public static void SetSpeed(this Animation anim, float newSpeed)
	{
		anim[anim.clip.name].speed = newSpeed; 
	}
	static public bool ApproxmatelyTest(this MonoBehaviour mb, float stamp1,float stamp2,float diff)
	{
		var v = Mathf.Abs(stamp1-stamp2);
		if(v>HoneyCombConstant.loopLength/2)
			return HoneyCombConstant.loopLength-v<diff;
		else
			return (v<diff);	
	}

	public static void myInvoke(this MonoBehaviour mb, float seconds,System.Action completeEvent)
	{
		mb.StartCoroutine(waitSeconds(seconds,completeEvent));
	}
	public static IEnumerator waitSeconds(float seconds,System.Action completeEvent)
	{
		yield return new WaitForSeconds(seconds);
		completeEvent();
	}
	private static System.Random rng = new System.Random();  

	public static void Shuffle<T>(this IList<T> list)  
	{  
		int n = list.Count;  
		while (n > 1) {  
			n--;  
			int k = rng.Next(n + 1);  
			T value = list[k];  
			list[k] = list[n];  
			list[n] = value;  
		}  
	}
}

