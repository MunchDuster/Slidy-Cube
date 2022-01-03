using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


public class WinLose : MonoBehaviour
{
	public Text finalScoreUI;

	[Header("Outer Refs")]
	public LevelGenerator levelGenerator;

	[Header("Events")]
	public UnityEvent OnWin;
	public UnityEvent OnLevelUp;
	public UnityEvent OnLose;

	private PlayerMovement movement;


	private void Start()
	{
		movement = GetComponent<PlayerMovement>();
	}

	private void OnCollisionEnter(Collision other)
	{

		if (other.gameObject.tag == "Untagged" || !movement.isGaming) return;
		else if (other.gameObject.tag == "Obstacle") Lose("Hit");
		else if (other.gameObject.tag == "Level") LevelUp();
		else if (other.gameObject.tag == "Finish") Win();
	}


	//Check if has fallen
	private void Update()
	{
		if (movement.isGaming && transform.position.y < levelGenerator.lowestPointY - 1f)
		{
			Lose("Fell");
		}
	}

	private void Lose(string reason)
	{
		Debug.Log("Player lost: " + reason);

		if (OnLose != null) OnLose.Invoke();

		//show the score text
		finalScoreUI.text = (transform.position.z * movement.scoreMultiplier).ToString("0.0");

		//reset time.timeScale
		Time.timeScale = 1;

		//relaod scene after 3 seconds
		StartCoroutine(WaitAndReload());
	}

	private void LevelUp()
	{
		if (OnLevelUp != null) OnLevelUp.Invoke();
	}
	private void Win()
	{

		if (OnWin != null) OnWin.Invoke();

		//reload scene after 3 seconds
		StartCoroutine(WaitAndReload());
	}

	private IEnumerator WaitAndReload()
	{
		yield return new WaitForSeconds(3);

		SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
	}
}
