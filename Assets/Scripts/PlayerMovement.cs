using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public float scoreMultiplier = 0.25f;
	public float forwardSpeed = 10;
	public float moveSideSpeed = 10;

	[HideInInspector]
	public bool isGaming = true;

	private Rigidbody rb;

	// Start is called before the first frame update
	private void Start()
	{
		rb = GetComponent<Rigidbody>();

		//Set Color of cube from settings gameobject
		GetComponent<MeshRenderer>().material.color = Settings.playerColor;
	}

	// Update is called once per frame
	private void FixedUpdate()
	{
		if (isGaming) rb.AddForce(new Vector3(Input.GetAxis("Horizontal") * moveSideSpeed - rb.velocity.x, 0, forwardSpeed - rb.velocity.z), ForceMode.VelocityChange);
	}
}