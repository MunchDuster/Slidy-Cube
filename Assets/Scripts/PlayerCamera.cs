using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
	[Header("Settings")]
	public float startingLerpSpeed = 4;
	public float normalLerpSpeed = 2;
	public float stoppingLerpSpeed = 4;
	public LayerMask layerMask;

	[Space(10)]
	public float cameraRotLerpSpeed = 4;

	[Space(10)]
	public float maxRayscastDistance = 3;

	[Space(10)]
	public Vector3 offset = new Vector3(0, 2, -5);


	[Header("Outer Refs")]
	public new Transform camera;
	public PlayerMovement movement;

	//Private Vars
	private Quaternion targetRotation;

	private float speedWhenStartedSmoothing;
	private delegate void OnEvent();
	private OnEvent OnUpdate;

	private Quaternion originalRotation;//Used by StartingCamera()
	private Vector3 originalPosition;//Used by StartingCamera()

	private Vector3 stoppingTarget; //Used by StoppingCamera()

	// Start is called before the first frame update
	void Start()
	{
		originalRotation = transform.rotation;
		originalPosition = transform.position;

		targetRotation = camera.rotation;

		camera.rotation = Settings.cameraRotation;
		camera.position = Settings.cameraPosition;

		OnUpdate += StartingCamera;
	}

	// Update is called once per frame
	private void LateUpdate()
	{
		OnUpdate();
	}

	//Used in main game
	private void NormalUpdateCamera()
	{
		UpdateCameraPosition();

		//Get camera target rotation from the movement
		targetRotation = Quaternion.LookRotation(movement.forward, -movement.down);

		camera.rotation = Quaternion.Slerp(camera.rotation, targetRotation, cameraRotLerpSpeed * Time.deltaTime);

		//State changed check
		if (!movement.isGaming)
		{
			stoppingTarget = camera.position + camera.forward * movement.rb.velocity.magnitude;

			Settings.cameraPosition = camera.position;
			Settings.cameraRotation = camera.rotation;

			OnUpdate -= NormalUpdateCamera;
			OnUpdate += StoppingCamera;
		}
	}


	private float totalStartLerp = 0;
	private void StartingCamera()
	{
		Vector3 targetPosition = transform.position + offset;

		camera.rotation = Quaternion.Lerp(originalRotation, targetRotation, totalStartLerp);
		camera.position = Vector3.Lerp(originalPosition, targetPosition, totalStartLerp);

		//State changed check
		if (totalStartLerp > 0.99f)
		{
			movement.isGaming = true;

			OnUpdate -= StartingCamera;
			OnUpdate += NormalUpdateCamera;
		}

		totalStartLerp += startingLerpSpeed * Time.deltaTime;

	}

	private float totalStoppingLerp = 0;
	private void StoppingCamera()
	{
		totalStoppingLerp = Mathf.Clamp01(totalStoppingLerp + stoppingLerpSpeed * Time.deltaTime);

		camera.position = Vector3.Lerp(camera.position, stoppingTarget, totalStoppingLerp);
	}

	//Used by NormalUpdateCamera() and StartingCamera()
	private void UpdateCameraPosition()
	{
		Vector3 rotatedOffset = Quaternion.LookRotation(movement.forward, -movement.down) * offset;
		Vector3 targetPos = transform.position + rotatedOffset;
		camera.position = Vector3.Lerp(camera.position, targetPos, normalLerpSpeed * Time.deltaTime);
	}
}