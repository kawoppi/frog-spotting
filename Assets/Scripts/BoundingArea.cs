using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * 
 */
public class BoundingArea : MonoBehaviour
{
	//frogautonomy requires a bounding area and sets up the callbacks

	public bool showIngame = false;

	public delegate void BoundingCallback(GameObject gameObject);
	public List<BoundingCallback> enterCallbacks;
	public List<BoundingCallback> exitCallbacks;

	void Awake()
	{
		GetComponent<MeshRenderer>().enabled = this.showIngame; //the reach area should only be visible in the editor
		enterCallbacks = new List<BoundingCallback>();
		exitCallbacks = new List<BoundingCallback>();
	}

	void OnTriggerEnter(Collider other)
	{
		foreach (BoundingCallback callback in enterCallbacks)
		{
			callback(other.gameObject);
		}
	}

	void OnTriggerExit(Collider other)
	{
		foreach (BoundingCallback callback in exitCallbacks)
		{
			callback(other.gameObject);
		}
	}
}
