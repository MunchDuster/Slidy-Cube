using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColorOnClick : MonoBehaviour
{
	public Gradient colors;
	public float speed = 0.2f;

	private float currentColor = 0;
	private void Start()
	{
		Settings.playerColor = gameObject.GetComponent<MeshRenderer>().material.color;
	}
	private void Update()
	{
		if (Input.GetMouseButton(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out RaycastHit hit, 100))
			{
				if (hit.transform == transform)
				{
					currentColor = (currentColor + speed * Time.deltaTime) % 1;
					Color color = colors.Evaluate(currentColor);

					gameObject.GetComponent<MeshRenderer>().material.color = color;

					Settings.playerColor = color;
				}
			}
		}
	}
}
