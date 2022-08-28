using UnityEngine;

public class LevelPiece : MonoBehaviour
{
	[Header("Inner Refs")]
	public Transform start;
	public Transform end;

	[HideInInspector] public LevelPiece lastPiece;

	//Used for calc of level progress
	[HideInInspector] public Level level;
	[HideInInspector] public float percentage;
	[HideInInspector] public float weight;


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

	//Used for calculation of level progress
	public float CalcPercentage(Vector3 position)
	{
		Vector3 direction = (end.position - start.position).normalized;

		float startPoint = Vector3.Project(start.position, direction).z;
		float endPoint = Vector3.Project(end.position, direction).z;

		float curPoint = Vector3.Project(position, direction).z;

		return Mathf.InverseLerp(startPoint, endPoint, curPoint);
	}
}