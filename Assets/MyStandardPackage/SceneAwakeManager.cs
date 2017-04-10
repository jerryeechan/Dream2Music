using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneAwakeManager : MonoBehaviour {
	public GameObject[] awakeAtStartObjects;
	void Awake()
	{
		foreach(var obj in awakeAtStartObjects)
		{
			obj.SetActive(true);
		}
	}
}