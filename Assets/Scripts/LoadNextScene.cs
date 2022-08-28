using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadNextScene : MonoBehaviour
{
	public TextMeshProUGUI[] fadeTexts;
	public new Transform camera;
	public Transform cameraTarget;
	public float lerpAmount = 0.5f;


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
			foreach (TextMeshProUGUI fadeText in fadeTexts)
			{
				fadeText.color -= new Color(0, 0, 0, 1 - totalLerp);
			}

			totalLerp += lerpAmount * Time.deltaTime * (1 - totalLerp);
			if (totalLerp > 0.99f)
			{
				//save current camera transform
				Settings.cameraPosition = camera.position;
				Settings.cameraRotation = camera.rotation;

				SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
			}
		}
	}
}
