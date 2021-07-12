using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeTrip : TripWire
{
	public Transform[] explodeTransforms;

	protected override void OnTrip()
	{
		//make explosions at all points
		foreach (Transform explodeTransform in explodeTransforms)
		{
			Vector3 point = explodeTransform.position;
			float radius = 3;

			Collider[] hitColliders = Physics.OverlapSphere(point, radius, ~LayerMask.GetMask("Player"));
			foreach (Collider col in hitColliders)
			{
				if (col.gameObject.GetComponent<Rigidbody>() != null)
				{
					col.gameObject.GetComponent<Rigidbody>().AddExplosionForce(2000, point, radius);
				}
			}
		}
	}
}
