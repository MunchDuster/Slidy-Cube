using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableTrip : TripWire
{
	public GameObject[] enableOnTrip;
	protected override void OnTrip()
	{
		foreach (GameObject objec in enableOnTrip)
		{
			objec.SetActive(true);
		}
	}
}
