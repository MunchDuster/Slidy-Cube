using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
	[Header("Settings")]
	public float startingLerpSpeed = 4;
	public float normalLerpSpeed = 2;
	public float stoppingLerpSpeed = 4;

	[Space(10)]
	public Vector3 offset = new Vector3(0, 2, -5);


	[Header("Outer Refs")]
	public new Transform camera;
	public PlayerMovement movement;

	//Private Vars
	private Quaternion originalRotation;

	private float speedWhenStartedSmoothing;

	private float stoppingLerp = 0;
	private float startingLerp = 0;

	private delegate void OnEvent();
	private OnEvent OnUpdate;


	// Start is called before the first frame update
	void Start()
	{
		originalRotation = camera.rotation;

		camera.rotation = Settings.cameraRotation;
		camera.position = Settings.cameraPosition;

		OnUpdate += StartingCamera;
	}

	// Update is called once per frame
	private void LateUpdate()
	{
		OnUpdate();
	}

	private void NormalUpdateCamera()
	{
		camera.position = Vector3.Lerp(camera.position, new Vector3(0, transform.position.y, transform.position.z) + offset, normalLerpSpeed * Time.deltaTime);

		//State changed check
		if (!movement.isGaming)
		{

			speedWhenStartedSmoothing = movement.forwardSpeed;
			stoppingLerp = 1;

			Settings.cameraPosition = camera.position;
			Settings.cameraRotation = camera.rotation;

			OnUpdate -= NormalUpdateCamera;
			OnUpdate += StoppingCamera;
		}
	}
	private void StartingCamera()
	{
		camera.rotation = Quaternion.Lerp(camera.rotation, originalRotation, startingLerpSpeed * Time.deltaTime);

		startingLerp += startingLerpSpeed * Time.deltaTime * (1 - startingLerp);

		//State changed check
		if (startingLerp > 0.99f)
		{
			OnUpdate -= StartingCamera;
			OnUpdate += NormalUpdateCamera;
		}
	}
	private void StoppingCamera()
	{
		stoppingLerp -= stoppingLerpSpeed * Time.deltaTime;
		camera.position = Vector3.Lerp(camera.position, camera.position + camera.forward * speedWhenStartedSmoothing, stoppingLerp * Time.deltaTime);
	}
}
