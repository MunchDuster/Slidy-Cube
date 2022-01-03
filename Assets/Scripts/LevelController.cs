using UnityEngine;

public class LevelController : MonoBehaviour
{
	public LevelGenerator levelGenerator;
	public delegate void OnLevelEvent(int levelNo);
	public OnLevelEvent OnFinishedLevel;

	public delegate void OnLevelCreated(float start, float end);
	public OnLevelCreated OnFinishedGeneratingLevel;

	public float levelSpeedBalance = 1.1f;

	private LevelEnd end;
	// Start is called before the first frame update
	void Start()
	{
		levelGenerator.OnFinishedGeneratingLevel += (float start, float end) => { if (OnFinishedGeneratingLevel != null) OnFinishedGeneratingLevel(start, end); };

		end = levelGenerator.GenerateLevel().end;
		end.tripwire.OnTripped += OnLevelCompleted;
	}

	private void OnLevelCompleted(GameObject caller)
	{
		end = levelGenerator.GenerateLevel().end;
		end.tripwire.OnTripped += OnLevelCompleted;

		//Increase level in settings
		Settings.level += 1;

		Time.timeScale = Mathf.Clamp(1f + (Settings.level - 1f) * levelSpeedBalance, 1f, 5f);
		Debug.Log("Level: " + Settings.level + " Speed: " + Time.timeScale);

		if (OnFinishedLevel != null) OnFinishedLevel(Settings.level);
	}
}
