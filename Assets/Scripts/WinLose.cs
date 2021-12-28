using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class WinLose : MonoBehaviour
{
	public Text finalScoreUI;

	[Header("Events")]
	public UnityEvent OnWin;
	public UnityEvent OnLose;

	[HideInInspector]
	public Transform winGate;


	private Movement movement;
	private Settings settings;


	private void Start()
	{
		GameObject settingsGameObject = GameObject.Find("Settings");
		settings = (settingsGameObject != null) ? settingsGameObject.GetComponent<Settings>() : null;

		movement = GetComponent<Movement>();
	}
	// Update is called once per frame
	void LateUpdate()
	{
		if (movement.isGaming)
		{
			if (transform.position.y < -1)
				Lose();
			if (winGate != null && transform.position.z >= winGate.position.z)
				Win();
		}
	}
	private void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.tag == "Obstacle") Lose();
	}
	private void Lose()
	{
		if (OnLose != null) OnLose.Invoke();

		if (movement.isGaming)
		{
			movement.isGaming = false;

			//show the score text
			finalScoreUI.text = ((int)(transform.position.z * movement.scoreMultiplier)).ToString();

			//make player have friction
			GetComponent<Collider>().material = null;

			//relaod scene after 3 seconds
			StartCoroutine(WaitAndReload());
		}

	}
	IEnumerator WaitAndReload()
	{
		yield return new WaitForSeconds(3);

		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	void Win()
	{
		if (OnWin != null) OnWin.Invoke();

		//player has won, enable isgaming to remove player input to cube and stop camera tracking and prevent losing/wining again
		movement.isGaming = false;

		//make player have friction
		GetComponent<Collider>().material = null;

		//reload scene after 3 seconds
		StartCoroutine(WaitAndReload());

		//Increase level in settings gameobject
		Settings.level += 1;
	}

}
