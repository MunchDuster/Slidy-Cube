using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TripWire : MonoBehaviour
{
	private bool haventGoneOff = true;

	private void OnTriggerEnter(Collider collider)
	{
		if (collider.gameObject.tag == "Player")
		{
			OnTrip(collider.gameObject);
		}
	}
	protected abstract void OnTrip(GameObject playerObj);
}
