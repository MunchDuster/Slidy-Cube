using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
	public float radius = 3;
	public float force = 2000;
	public LayerMask layerMask;

	public void ExplodeAt(Vector3 point)
	{
		Collider[] hitColliders = Physics.OverlapSphere(point, radius, layerMask);
		foreach (Collider col in hitColliders)
		{
			Rigidbody rb = col.gameObject.GetComponent<Rigidbody>();

			if (rb != null)
			{
				rb.AddExplosionForce(force, point, radius);
			}
		}
	}

	public void ExplodeAll(Transform pointsParent)
	{
		//make explosions at all points
		foreach (Transform explodeTransform in pointsParent)
		{
			ExplodeAt(explodeTransform.position);
		}
	}
}
