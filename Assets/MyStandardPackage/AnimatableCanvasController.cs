using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatableCanvasController : MonoBehaviour {

	public AnimatableCanvas aCanvas;
	void Awake()
	{
		aCanvas = GetComponentInChildren<AnimatableCanvas>();
	}
}
