using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
	public float scoreMultiplier = 0.25f;
	public float forwardSpeed = 10;
	public float moveSideSpeed = 10;
	public float jumpHeight = 0.6f;

	[Space(10)]
	public float gravity;

	[Space(10)]
	public float antiSpinTorque = 200;
	public float dampingAmount = 100;
	public float maxDamp = 50;

	[Space(10)]
	public PlayerInput input;
	public LayerMask layerMask;

	[HideInInspector] public bool isGaming = false;
	[HideInInspector] public Rigidbody rb;

	[HideInInspector] public Vector3 forward = Vector3.forward, right = Vector3.right, down = -Vector3.up;

	private bool isGrounded, hasJumped;
	private Vector3 lastPos;

	// Start is called before the first frame update
	private void Start()
	{
		rb = GetComponent<Rigidbody>();

		//Set Color of cube from settings gameobject
		GetComponent<MeshRenderer>().material.color = Settings.playerColor;

		//Init lastPos
		lastPos = transform.position;
	}

	//For UnityEvents to set isGaming
	public void SetIsGaming(bool isGaming) { this.isGaming = isGaming; }

	// FixedUpdate is called once per physics loop
	private void FixedUpdate()
	{
		if (isGaming)
		{
			UpdateDirections();
			ApplyForces();
			ApplyTorques();
		}

		//Debug line of positions of cube
		Debug.DrawLine(lastPos, transform.position, Color.blue, 10);
		lastPos = transform.position;
	}

	//Move forward, side and gravity
	private void ApplyForces()
	{
		//Move forward and side
		float moveSide = input.side * moveSideSpeed;
		float moveForward = forwardSpeed;

		float jump = 0;

		if (input.pendingJump && !hasJumped)
		{
			input.pendingJump = false;
			hasJumped = true;

			//JumpForce = SquareRoot(Gravity * JumpHeight * 2)
			jump = Mathf.Sqrt(gravity * jumpHeight * 2);
		}

		Vector3 moveForce = forward * moveForward + right * moveSide + jump * -down;

		Vector3 antiSlide = Vector3.Project(rb.velocity, forward) + Vector3.Project(rb.velocity, right);

		rb.AddForce(moveForce - antiSlide, ForceMode.VelocityChange);

		//Gravitational acceleration
		Vector3 gravityForce = gravity * down;
		rb.AddForce(gravityForce, ForceMode.Acceleration);
	}

	//Alignment torque
	private void ApplyTorques()
	{
		Vector3 angleBasis = forward;
		Vector3 axis = -down;

		//alignment
		Vector3 yTorque = AlignAxis(forward, transform.forward, -down);
		Vector3 xTorque = Vector3.zero; //AlignAxis(right, transform.right, right);

		Vector3 spinTorque = xTorque + yTorque;

		rb.AddTorque(spinTorque * Time.fixedDeltaTime);
	}
	private Vector3 AlignAxis(Vector3 angle0, Vector3 angle1, Vector3 axis)
	{
		float angle = Vector3.SignedAngle(angle0, angle1, axis) % 180;

		float angularVelocity = Vector3.Project(rb.angularVelocity, axis).y;

		float damping = angularVelocity * dampingAmount;
		float reAlignmentTorque = -angle * antiSpinTorque + damping;

		return axis * reAlignmentTorque;
	}

	//Re-evaluate directions based on raycast (direction: current down).
	private void UpdateDirections()
	{
		if (Physics.Raycast(transform.position, down, out RaycastHit hit, 20f, layerMask))
		{
			Vector3 normal = hit.normal;

			if (hit.transform.parent == null) return;
			LevelPiece levelPiece = hit.transform.parent.GetComponent<LevelPiece>();

			if (levelPiece != null)
			{
				forward = levelPiece.end.forward;
				right = levelPiece.end.right;
				down = -levelPiece.end.up;

			}
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.layer == layerMask) isGrounded = true;
	}
	private void OnCollisionExit(Collision collision)
	{
		if (collision.gameObject.layer == layerMask) isGrounded = false;
	}
}