using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public float scoreMultiplier = 0.25f;
	public float forwardSpeed = 10;
	public float moveSideSpeed = 10;

	[Space(10)]
	public float antiSpinTorque = 1;

	[HideInInspector]
	public bool isGaming = false;
	[HideInInspector]
	public Rigidbody rb;

	// Start is called before the first frame update
	private void Start()
	{
		rb = GetComponent<Rigidbody>();

		//Set Color of cube from settings gameobject
		GetComponent<MeshRenderer>().material.color = Settings.playerColor;
	}

	// FixedUpdate is called once per physics loop
	private void FixedUpdate()
	{
		if (isGaming)
		{
			//Force
			float moveForward = Input.GetAxis("Horizontal") * moveSideSpeed - rb.velocity.x;
			float moveSide = forwardSpeed - rb.velocity.z;

			Vector3 moveForce = new Vector3(moveForward, 0, moveSide);

			rb.AddForce(moveForce, ForceMode.VelocityChange);

			//Torque
			float reAlignmentTorque = -transform.rotation.y * antiSpinTorque - rb.angularVelocity.y;

			Vector3 spinTorque = Vector3.up * reAlignmentTorque;

			rb.AddTorque(spinTorque);

		}
	}
}