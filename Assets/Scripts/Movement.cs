using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
	public float scoreMultiplier = 0.25f;
	public float forwardSpeed = 10;
	public float moveSideSpeed = 10;
	public float slowDownSpeed = 2;
	public Vector3 offset = new Vector3(0, 3, -3);
	public new Transform camera;
	[Header("Level Settings")]
	public float speedMultiplierIncreasePerLevel = 0.2f;


	[HideInInspector]
	public bool isGaming = true;

	[Header("UI")]
	public Text scoreUI;
	public Text levelUI;


	private Rigidbody rb;
	private float levelSpeedMultiplier;
	private bool isLerping = true; //Lerp camera to offset
	Settings settings;
	// Start is called before the first frame update
	void Start()
	{
		rb = GetComponent<Rigidbody>();
		originalRotation = camera.rotation;
		//Set Color of cube frm settings gameobject
		settings = (GameObject.Find("Settings") != null) ? GameObject.Find("Settings").GetComponent<Settings>() : null;
		if (settings != null)
		{
			levelSpeedMultiplier = 1 + speedMultiplierIncreasePerLevel * settings.level;
			levelUI.text = "Level " + (settings.level + 1);

			camera.rotation = settings.cameraRotation;
			camera.position = settings.cameraPosition;

			GetComponent<MeshRenderer>().material.color = settings.playerColor;
		}
		else
		{
			levelSpeedMultiplier = 1;
		}
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		if (isGaming && !isLerping) rb.AddForce(new Vector3(Input.GetAxis("Horizontal") * moveSideSpeed * levelSpeedMultiplier - rb.velocity.x, 0, forwardSpeed * levelSpeedMultiplier - rb.velocity.z), ForceMode.VelocityChange);
	}
	Quaternion originalRotation;
	private float speedWhenStartedSmoothing;
	private float lerp;
	bool wasGaming = true;
	float totalLerp = 0;
	float lerpAmount = 4;
	private void LateUpdate()
	{
		if (isLerping)
		{
			camera.position = Vector3.Lerp(camera.position, new Vector3(0, 0, transform.position.z) + offset, lerpAmount * Time.deltaTime);
			camera.rotation = Quaternion.Lerp(camera.rotation, originalRotation, lerpAmount * Time.deltaTime);

			totalLerp += lerpAmount * Time.deltaTime * (1 - totalLerp);
			if (totalLerp > 0.99f)
			{
				isLerping = false;
			}
		}
		else
		{
			if (isGaming)
			{
				camera.position = new Vector3(0, 0, transform.position.z) + offset;
				scoreUI.text = ((int)(transform.position.z * scoreMultiplier)).ToString();

			}
			else
			{
				if (wasGaming)
				{
					wasGaming = false;
					speedWhenStartedSmoothing = forwardSpeed;
					lerp = 1;
					//Increase level in settings gameobject
					if (settings != null)
					{
						settings.cameraPosition = camera.position;
						settings.cameraRotation = camera.rotation;
					}
				}
				lerp -= slowDownSpeed * Time.deltaTime;
				camera.position = Vector3.Lerp(camera.position, camera.position + camera.forward * speedWhenStartedSmoothing, lerp * Time.deltaTime);

			}
		}

	}

	private float roundDigits(float num)
	{
		return (int)(num * 10) / 10f;
	}
}
