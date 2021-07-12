using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{
	public float force = 10;

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			other.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, force, 0), ForceMode.VelocityChange);
		}
	}
}
