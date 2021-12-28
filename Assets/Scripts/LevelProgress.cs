using UnityEngine;
using UnityEngine.UI;

public class LevelProgress : MonoBehaviour
{
	public ObstacleGenerator obstacleGenerator;
	public Slider slider;
	public Transform playerTransform;

	// Start is called before the first frame update
	void Start()
	{
		obstacleGenerator.OnFinishedGeneratingLevel += (float levelSize) => { slider.maxValue = levelSize; };
	}

	// Update is called once per frame
	void Update()
	{
		slider.value = playerTransform.position.z;
	}
}
