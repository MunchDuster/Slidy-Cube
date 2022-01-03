using UnityEngine;

public class PlayerInput : MonoBehaviour
{
	[HideInInspector] public float side;
	[HideInInspector] public bool pendingJump;

	//Update is called every frame.
	private void Update()
	{
		side = Input.GetAxis("Horizontal");
		if (Input.GetKeyDown(KeyCode.Space))
		{
			pendingJump = true;
		}
	}
}