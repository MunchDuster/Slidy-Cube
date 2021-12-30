using System.Collections.Generic;
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
		public LevelPiece part1;
		public LevelPiece part2;


		public bool Contains(LevelPiece ob1, LevelPiece ob2)
		{
			return (part1 == ob1 && part2 == ob2) || (part1 == ob2 && part2 == ob1);
		}
	}

	[Header("Main settings")]
	public int obstaclePrefabsToSpawn = 20;
	[Range(0f, 1f)]
	public float bonusPrefabsSpawnChance = 0.2f;

	public float obstacleIncreasePerLevel = 2;

	[Header("Inner Refs")]
	public LevelPiece startGamePiece;
	public LevelPiece levelEndPrefab;

	[Space(10)]

	public LevelPiece[] obstaclePrefabs;
	public GameObject[] bonusPrefabs;

	public BadCombo[] badCombos;


	public delegate void OnLevelEvent(float start, float end);
	public OnLevelEvent OnFinishedGeneratingLevel;

	private float _totalDistance = 0;
	private float totalDistance { get { return _totalDistance; } }
	private float lastLevelEnd = 0;

	private LevelPiece lastObstacle;

	private void Awake()
	{
		Init();
	}
	private void Init()
	{
		//Set random last obstacle
		lastObstacle = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length - 1)];
	}

	public LevelEnd GenerateLevel()
	{
		lastLevelEnd = totalDistance;

		obstaclePrefabsToSpawn += (int)(obstacleIncreasePerLevel * Settings.level);

		float startDistance = startGamePiece.length;
		_totalDistance += startDistance;

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

	private LevelEnd SpawnEnd()
	{
		Vector3 point = new Vector3(0, 0, totalDistance);

		//Spawn the end part
		GameObject endSpawn = Instantiate(levelEndPrefab.gameObject, point, Quaternion.identity, transform);

		LevelEnd levelEnd = endSpawn.GetComponent<LevelEnd>();
		//Get the trigger point on the end
		Transform triggerPoint = levelEnd.triggerPoint;

		//Call OnFinishedGeneratingLevel event
		if (OnFinishedGeneratingLevel != null) OnFinishedGeneratingLevel(lastLevelEnd, triggerPoint.position.z);

		return levelEnd;
	}
	private void SpawnObstacle()
	{
		LevelPiece chosenObstacle = ChooseObstacle();
		Vector3 point = new Vector3(0, 0, totalDistance);

		GameObject spawnObj = Instantiate(chosenObstacle.gameObject, point, Quaternion.identity, transform);

		LevelPiece obstacle = spawnObj.GetComponent<LevelPiece>();

		_totalDistance += obstacle.length;
	}
	private void SpawnBonus()
	{
		GameObject prefab = bonusPrefabs[(int)Mathf.Round(Random.Range(0f, bonusPrefabs.Length - 1f))];
		Vector3 position = new Vector3(Random.Range(-4, 4), 0, totalDistance);

		Instantiate(prefab, position, Quaternion.identity, transform);
	}

	private LevelPiece ChooseObstacle()
	{
		///Make list of available obstacles///

		//Create copy of obstacles array
		List<LevelPiece> availableObstacles = obstaclePrefabs.Clone() as List<LevelPiece>;

		Debug.Log("List of available obstacles: " + availableObstacles);
		Debug.Log("Length of available obstacles: " + availableObstacles.Count);

		//Filter out bad combos
		foreach (LevelPiece obstacle in availableObstacles)
		{
			foreach (BadCombo badCombo in badCombos)
			{
				if (badCombo.Contains(lastObstacle, obstacle))
				{
					availableObstacles.Remove(obstacle);
				}
			}
		}

		///Choose random from available///

		//Choose random index
		int index = Random.Range(0, availableObstacles.Count);

		//Return chosen by index
		return availableObstacles[index];
	}
}
