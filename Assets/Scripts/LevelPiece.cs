using UnityEngine;

public class LevelPiece : MonoBehaviour
{
	[Header("Inner Refs")]
	public Transform start;
	public Transform end;

	public float length
	{
		get
		{
			return (end.position - start.position).magnitude;
		}
	}
}