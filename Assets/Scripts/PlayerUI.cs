using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
	[Header("Inner Refs")]
	public Slider levelProgressSlider;
	public TextMeshProUGUI levelText;
	public TextMeshProUGUI scoreText;
	public Animator animator;
	public GameObject[] strikes;


	[Header("Outer Refs")]
	public LevelController levelController;
	public Transform playerTransform;
	public PlayerMovement movement;
	public WinLose winLose;

	[HideInInspector] public LevelPiece curPiece;

	// Start is called before the first frame update
	void Awake()
	{
		levelController.OnFinishedLevel += OnFinishedLevel;
		levelController.OnFinishedGeneratingLevel += OnFinishedGeneratingLevel;

		levelText.text = "Level " + Settings.level;

		winLose.OnStrike.AddListener(() => { OnStrike(); });
	}

	// Called when player gets a strike
	private void OnStrike()
	{
		if (winLose.strikesHad > winLose.strikesHad - 1)
			strikes[winLose.strikesHad - 1].SetActive(true);
	}

	private void OnFinishedGeneratingLevel(float levelStart, float levelEnd)
	{
		levelProgressSlider.minValue = levelStart;
		levelProgressSlider.maxValue = levelEnd;
	}
	private void OnFinishedLevel(int levelNo)
	{
		levelText.text = "Level " + levelNo;

		animator.ResetTrigger("LevelUp");
		animator.SetTrigger("LevelUp");
	}

	// Update is called once per frame
	private void Update()
	{
		levelProgressSlider.value = GetLevelProgress();
	}

	// FixedUpdate is called every physics update
	private void FixedUpdate()
	{
		if (Physics.Raycast(transform.position, movement.down, out RaycastHit hit, 10, movement.layerMask))
		{
			LevelPiece piece = hit.transform.GetComponent<LevelPiece>();

			if (piece != null) curPiece = piece;
		}
	}

	//Find the level progress
	private float GetLevelProgress()
	{
		if (curPiece == null) return 0;

		float overallLevelProgress = curPiece.percentage;

		float pieceProgress = curPiece.CalcPercentage(transform.position);

		float pieceWeight = curPiece.weight;

		return overallLevelProgress + pieceProgress * pieceWeight;
	}

}
