using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
	public Vector3 offset;
	public float rotationSpeed;
	public new Transform camera;


	private float angle = 0;
	private Settings settings;
	private void Start()
	{
		//Increase level in settings gameobject
		settings = (GameObject.Find("Settings") != null) ? GameObject.Find("Settings").GetComponent<Settings>() : null;
	}
	private void LateUpdate()
	{

		angle += Time.deltaTime * rotationSpeed;

		camera.position = rotateVector(offset, angle);
		camera.LookAt(transform);
	}
	private Vector3 rotateVector(Vector3 vector, float angle)
	{
		return Quaternion.AngleAxis(angle, Vector3.up) * vector;
	}
}
