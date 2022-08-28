using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


public class WinLose : MonoBehaviour
{
	public bool doNormalStuffOnLose = true;
	public int strikes;


	[Header("Outer Refs")]
	public LevelGenerator levelGenerator;

	[Header("Events")]
	public UnityEvent OnWin;
	public UnityEvent OnLevelUp;
	public UnityEvent OnLose;
	public UnityEvent OnStrike;


	private PlayerMovement movement;

	//For keeping track of strikes taken
	private int strikesLeft;

	//For classes that want to know how many strikes have occured
	public int strikesHad { get { return strikes - strikesLeft; } }

	//For calculating end score
	private float totalDistanceTravelled = 0;
	private Vector3 lastPos;

	private void Start()
	{
		movement = GetComponent<PlayerMovement>();
		lastPos = transform.position;

		strikesLeft = strikes;
	}

	private void OnCollisionEnter(Collision other)
	{

		if (other.gameObject.tag == "Untagged" || !movement.isGaming) return;
		else if (other.gameObject.tag == "Obstacle") TakeStrike("Hit");
		else if (other.gameObject.tag == "Level") LevelUp();
		else if (other.gameObject.tag == "Finish") Win();
	}


	//Check if has fallen
	private void Update()
	{
		if (movement.isGaming)
		{
			if (CheckHasFallen())
			{
				TakeStrike("Fell");
			}
			else
			{
				totalDistanceTravelled += Vector3.Project(transform.position - lastPos, movement.forward).magnitude;
				lastPos = transform.position;
			}
		}
	}

	public float timeUntilFallen = 2;
	private float timeFalling = 0;
	private bool CheckHasFallen()
	{
		bool groundUnderneath = Physics.Raycast(transform.position, movement.down, 200, movement.layerMask);
		if (!groundUnderneath)
		{
			timeFalling += Time.deltaTime;
			if (timeFalling > timeUntilFallen) return true;
		}
		else
		{
			timeFalling = 0;
		}

		return false;
	}
	public float GetScore()
	{
		return totalDistanceTravelled * movement.scoreMultiplier;
	}

	private void TakeStrike(string reason)
	{
		Debug.Log("Player lost strike: " + reason);

		strikesLeft--;

		if (OnStrike != null) OnStrike.Invoke();

		if (strikesLeft <= 0)
		{
			Lose();

		}
	}
	private void Lose()
	{
		Debug.Log("Player lost");

		if (OnLose != null) OnLose.Invoke();

		if (!doNormalStuffOnLose) return;

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
