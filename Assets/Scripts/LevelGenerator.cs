using UnityEngine;

/*
TODO:

Make this use multithreading - so user can play while loads ahead
*/
public class LevelGenerator : MonoBehaviour
{
	[System.Serializable]
	public struct BadCombo
	{
		public GameObject part1;
		public GameObject part2;
	}

	[Header("Main settings")]
	public float startDistance = 30;
	public float spawnDistance = 10;
	public int obstaclePrefabsToSpawn = 20;
	public float groundEndSpace = 10;
	public float groundStartSpace = 5;
	[Range(0f, 1f)]
	public float bonusPrefabsSpawnChance = 0.2f;

	[Header("Level Settings")]
	public float obstacleIncreasePerLevel = 2;

	public GameObject[] obstaclePrefabs;
	public GameObject[] bonusPrefabs;

	public BadCombo pairToAvoid;
	public GameObject levelEndPrefab;

	[Header("Refs")]
	public Transform groundTransform;
	public Renderer groundRenderer;

	public delegate void OnLevelEvent(float start, float end);
	public OnLevelEvent OnFinishedGeneratingLevel;

	private float _totalDistance = 0;
	private float totalDistance { get { return _totalDistance; } }
	private float lastLevelEnd = 0;

	private void Awake()
	{
		Init();
	}
	private void Init()
	{
		//Init the ground//
		//set the space behind the player
		groundTransform.localScale = new Vector3(1, 1, groundStartSpace);
		//make it so that the ground ends at z = 0
		groundTransform.position = new Vector3(0, 0, -groundStartSpace / 2);
		//add more tiling the texture to match scale increase
		groundRenderer.material.mainTextureScale += new Vector2(0, groundStartSpace);

		//Set random last obstacle
		lastObstacleIndex = Random.Range(0, obstaclePrefabs.Length - 1);
	}

	public LevelEnd GenerateLevel()
	{
		lastLevelEnd = totalDistance;

		obstaclePrefabsToSpawn += (int)(obstacleIncreasePerLevel * Settings.level);

		IncreaseGroundDistance(startDistance);

		for (int i = 0; i < obstaclePrefabs.Length; i++)
		{
			SpawnObstacle();

			if (Random.Range(0f, 1f) <= bonusPrefabsSpawnChance)
			{
				SpawnBonus();
			}
		}
		return SpawnEnd();
	}
	// Update is called once per frame
	int spawnedObstacles = 0;
	int lastObstacleIndex;
	bool done = false;

	private LevelEnd SpawnEnd()
	{
		done = true;

		Vector3 point = new Vector3(0, 0, totalDistance);

		//Spawn the end part
		GameObject endSpawn = Instantiate(levelEndPrefab, point, Quaternion.identity, transform);

		LevelEnd levelEnd = endSpawn.GetComponent<LevelEnd>();
		//Get the trigger point on the end
		Transform triggerPoint = levelEnd.triggerPoint;

		//Call OnFinishedGeneratingLevel event
		if (OnFinishedGeneratingLevel != null) OnFinishedGeneratingLevel(lastLevelEnd, triggerPoint.position.z);

		return levelEnd;
	}
	private int getIndex(int top, int exception)
	{
		int num = Random.Range(0, top - 1);
		return (num >= exception) ? num + 1 : num;
	}
	private void SpawnObstacle()
	{
		spawnedObstacles++;

		GameObject chosenObstacle = ChooseObstacle();
		Vector3 point = new Vector3(0, 0, totalDistance);

		GameObject spawnObj = Instantiate(chosenObstacle, point, Quaternion.identity, transform);

		Obstacle obstacle = spawnObj.GetComponent<Obstacle>();

		IncreaseGroundDistance(spawnDistance + obstacle.thickness);
	}
	private void SpawnBonus()
	{
		GameObject prefab = bonusPrefabs[(int)Mathf.Round(Random.Range(0f, bonusPrefabs.Length - 1f))];
		Vector3 position = new Vector3(Random.Range(-4, 4), 0, totalDistance);

		Instantiate(prefab, position, Quaternion.identity, transform);
	}
	private void IncreaseGroundDistance(float distance)
	{
		//move it forward half its size increase
		groundTransform.localScale += new Vector3(0, 0, distance);
		//make it so that the ground ends at z = 0
		groundTransform.position += new Vector3(0, 0, distance / 2);
		//add more tiling the texture to match scale increase
		groundRenderer.material.mainTextureScale += new Vector2(0, distance);

		_totalDistance += distance;
	}
	private GameObject ChooseObstacle()
	{
		//Choose obstacle
		int index = 0;
		if (obstaclePrefabs[lastObstacleIndex] == pairToAvoid.part1)
		{
			for (int i = 0; i < obstaclePrefabs.Length; i++)
			{
				if (obstaclePrefabs[i] == pairToAvoid.part2)
					index = getIndex(obstaclePrefabs.Length - 1, i);
			}

		}
		else if (obstaclePrefabs[lastObstacleIndex] == pairToAvoid.part2)
		{
			for (int i = 0; i < obstaclePrefabs.Length; i++)
			{
				if (obstaclePrefabs[i] == pairToAvoid.part1)
					index = getIndex(obstaclePrefabs.Length - 1, i);
			}

		}
		else
			index = getIndex(obstaclePrefabs.Length - 1, lastObstacleIndex);

		lastObstacleIndex = index;

		return obstaclePrefabs[index];
	}
}
