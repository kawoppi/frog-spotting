using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BoundingArea lets you receive trigger events with this GameObject on a script inside of a different GameObject.
/// Callbacks must be added during or after Start(), because the lists for them are initialized in Awake().
/// </summary>
public class BoundingArea : MonoBehaviour
{
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
