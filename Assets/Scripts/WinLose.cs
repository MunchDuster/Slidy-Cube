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
	public UnityEvent OnLevelUp;
	public UnityEvent OnLose;

	private PlayerMovement movement;


	private void Start()
	{
		movement = GetComponent<PlayerMovement>();
	}
	// Update is called once per frame
	private void LateUpdate()
	{
		if (movement.isGaming)
		{
			if (transform.position.y < -1)
				Lose();
		}
	}
	private void OnCollisionEnter(Collision other)
	{

		if (other.gameObject.tag == "Untagged" || !movement.isGaming) return;
		else if (other.gameObject.tag == "Obstacle") Lose();
		else if (other.gameObject.tag == "Level") LevelUp();
		else if (other.gameObject.tag == "Finish") Win();
	}

	private void Lose()
	{
		if (movement.isGaming)
		{
			movement.isGaming = false;

			if (OnLose != null) OnLose.Invoke();

			//show the score text
			finalScoreUI.text = ((int)(transform.position.z * movement.scoreMultiplier)).ToString();

			//reset time.timeScale
			Time.timeScale = 1;

			Settings.level = 1;

			//make player have friction
			GetComponent<Collider>().material = null;

			//relaod scene after 3 seconds
			StartCoroutine(WaitAndReload());
		}
	}
	private void LevelUp()
	{
		if (OnLevelUp != null) OnLevelUp.Invoke();
	}
	private void Win()
	{
		if (OnWin != null) OnWin.Invoke();

		//Reset level
		Settings.level = 1;

		//reload scene after 3 seconds
		StartCoroutine(WaitAndReload());
	}

	private IEnumerator WaitAndReload()
	{
		yield return new WaitForSeconds(3);

		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}
