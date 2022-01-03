using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CustomGravity : MonoBehaviour
{
	public Vector3 direction = Vector3.down;
	public float magnitude = 9.81f;

	private Rigidbody rb;

	// Start is called before the first frame update
	void Start()
	{
		rb = GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		rb.AddForce(direction * magnitude, ForceMode.Acceleration);
	}
}
