using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Movement : MonoBehaviour
{
	public float scoreMultiplier = 0.25f;
	public float forwardSpeed = 10;
	public float moveSideSpeed = 10;
	public float slowDownSpeed = 2;
	public Vector3 offset = new Vector3(0, 3, -3);
	public new Transform camera;

	[HideInInspector]
	public bool isGaming = true;

	[Header("UI")]
	public TextMeshProUGUI levelUI;

	private Rigidbody rb;
	private bool isLerping = true; //Lerp camera to offset

	// Start is called before the first frame update
	private void Start()
	{
		rb = GetComponent<Rigidbody>();
		originalRotation = camera.rotation;


		levelUI.text = "Level " + (Settings.level + 1);

		camera.rotation = Settings.cameraRotation;
		camera.position = Settings.cameraPosition;

		//Set Color of cube frm settings gameobject
		GetComponent<MeshRenderer>().material.color = Settings.playerColor;
	}

	// Update is called once per frame
	private void FixedUpdate()
	{
		if (isGaming && !isLerping) rb.AddForce(new Vector3(Input.GetAxis("Horizontal") * moveSideSpeed - rb.velocity.x, 0, forwardSpeed - rb.velocity.z), ForceMode.VelocityChange);
	}

	private Quaternion originalRotation;
	private float speedWhenStartedSmoothing;
	private float lerp;
	private bool wasGaming = true;
	private float totalLerp = 0;
	private float lerpAmount = 4;

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

			}
			else
			{
				if (wasGaming)
				{
					wasGaming = false;
					speedWhenStartedSmoothing = forwardSpeed;
					lerp = 1;

					Settings.cameraPosition = camera.position;
					Settings.cameraRotation = camera.rotation;
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