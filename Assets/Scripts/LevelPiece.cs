using UnityEngine;

public class LevelPiece : MonoBehaviour
{
	[Header("Inner Refs")]
	public Transform start;
	public Transform end;

	[HideInInspector]
	public LevelPiece lastPiece;

	public float length
	{
		get
		{
			return (end.position - start.position).magnitude;
		}
	}

	public Transform cameraAngle;


	// Start is called before the first frame update
	private void Start()
	{
		if (lastPiece == null) return;

		/*Quick fix for fixing point matching issues when piece is rotated*/
		Vector3 rotatedOffset = transform.position - start.position;
		Vector3 rotatedPoint = lastPiece.end.position + rotatedOffset;
		transform.position = rotatedPoint;

		//Adjust gravity of all child blocks
		foreach (CustomGravity gravity in GetComponentsInChildren<CustomGravity>())
		{
			gravity.direction = -transform.up;
		}
	}
}