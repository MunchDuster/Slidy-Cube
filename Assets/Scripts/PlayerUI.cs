using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
	[Header("Inner Refs")]
	public Slider levelProgressSlider;
	public TextMeshProUGUI levelText;
	public Animator animator;


	[Header("Outer Refs")]
	public LevelController levelController;
	public Transform playerTransform;

	// Start is called before the first frame update
	void Awake()
	{
		levelController.OnFinishedLevel += OnFinishedLevel;
		levelController.OnFinishedGeneratingLevel += OnFinishedGeneratingLevel;

		levelText.text = "Level " + Settings.level;
	}
	private void OnFinishedGeneratingLevel(float levelStart, float levelEnd)
	{
		Debug.Log("UI Finished Gen Level");
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
	void Update()
	{
		levelProgressSlider.value = playerTransform.position.z;
	}
}
