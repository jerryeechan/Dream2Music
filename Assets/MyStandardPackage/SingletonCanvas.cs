using System.Collections;
using UnityEngine;

public class SingletonCanvas<T> : AnimatableCanvas where T : AnimatableCanvas
{
	protected static T _instance;
	override protected void Awake()
	{
		_instance = GetComponent<T>();
		base.Awake();
		
	}
	/**
      Returns the instance of this singleton.
   */
	public static T instance
	{
		get
		{
			if(_instance == null)
			{
				_instance = (T) FindObjectOfType(typeof(T));
				
				if (_instance == null)
				{
					Debug.LogError("An instance of " + typeof(T) + 
					               " is needed in the scene, but there is none.");
				}
			}
			
			return _instance;
		}
	}
}