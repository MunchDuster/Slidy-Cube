using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
	public Color playerColor;
	public int level = 0;
	public Vector3 cameraPosition;
	public Quaternion cameraRotation;
	private void Awake()
	{
		DontDestroyOnLoad(this.gameObject);
	}
}
