using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinLose : MonoBehaviour
{
	public GameObject[] enableOnWin;
	public GameObject[] disableOnWin;
	public GameObject[] enableOnLose;
	public GameObject[] disableOnLose;
	public Text finalScoreUI;

	private Movement movement;

	[HideInInspector]
	public Transform winGate;

	private Settings settings;
	private void Start()
	{

		settings = (GameObject.Find("Settings") != null) ? GameObject.Find("Settings").GetComponent<Settings>() : null;

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
		if (movement.isGaming)
		{
			movement.isGaming = false;
			//enable lose screen
			foreach (GameObject loseScreen in enableOnLose)
				loseScreen.SetActive(true);


			//disable all gameobjects in the disableOnLose list
			foreach (GameObject thiing in disableOnLose)
				thiing.SetActive(false);

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
		//player has won, enable isgaming to remove player input to cube and stop camera tracking and prevent losing/wining again
		movement.isGaming = false;

		//enable all enableOnWin
		foreach (GameObject win in enableOnWin)
			win.SetActive(true);

		//enable all enableOnWin
		foreach (GameObject win in disableOnWin)
			win.SetActive(false);

		//make player have friction
		GetComponent<Collider>().material = null;

		//relaod scene after 3 seconds
		StartCoroutine(WaitAndReload());



		//Increase level in settings gameobject
		if (settings != null) settings.level += 1;
	}

}
