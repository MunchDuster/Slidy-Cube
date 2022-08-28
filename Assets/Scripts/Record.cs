using UnityEngine;

public class Record { };
public class WorldRecord : Record
{
	public Vector3 position;
	public Quaternion rotation;
	public Transform transform;

	public WorldRecord(Transform transform)
	{
		position = transform.position;
		rotation = transform.rotation;
		this.transform = transform;
	}
}
public class DestroyRecord : Record { }