using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadNextScene : MonoBehaviour
{
	public Text[] fadeTexts;
	public new Transform camera;
	public Transform cameraTarget;
	public float lerpAmount = 0.5f;
	public CameraRotate cameraRotate;


	private bool isLerping = false;
	private float angleIncreaseNeeded;
	public void LoadNextSceneNow()
	{
		isLerping = true;
		angleIncreaseNeeded = camera.rotation.eulerAngles.y;
		Debug.Log(angleIncreaseNeeded);
	}
	float totalLerp = 0;
	private void LateUpdate()
	{
		if (isLerping)
		{
			foreach (Text fadeText in fadeTexts)
				fadeText.color -= new Color(0, 0, 0, 1 - totalLerp);
			cameraRotate.offset -= Vector3.Scale(cameraRotate.offset, Vector3.forward * Time.deltaTime * lerpAmount);
			cameraRotate.rotationSpeed = angleIncreaseNeeded;
			totalLerp += lerpAmount * Time.deltaTime * (1 - totalLerp);
			if (totalLerp > 0.99f)
			{
				//save current camera transform
				Settings settings = (GameObject.Find("Settings") != null) ? GameObject.Find("Settings").GetComponent<Settings>() : null;
				settings.cameraPosition = camera.position;
				settings.cameraRotation = camera.rotation;


				SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
			}
		}
	}
}
