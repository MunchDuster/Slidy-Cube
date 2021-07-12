using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TripWire : MonoBehaviour
{
	[HideInInspector]
	public GameObject player;

	private bool haventGoneOff = true;
	private Movement playerMovement;

	private void Update()
	{
		if (player != null)
		{
			if (playerMovement == null)
				playerMovement = player.GetComponent<Movement>();

			if (player.transform.position.z >= transform.position.z && haventGoneOff && playerMovement.isGaming)
			{
				haventGoneOff = false;
				OnTrip();
			}
		}
	}
	protected abstract void OnTrip();
}
