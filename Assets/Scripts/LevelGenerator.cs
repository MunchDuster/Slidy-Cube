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

	[Header("Outer Refs")]
	public Transform pieceParent;


	public delegate void OnLevelEvent(float start, float end);
	public OnLevelEvent OnFinishedGeneratingLevel;

	private float _totalDistance = 0;
	private float totalDistance { get { return _totalDistance; } }
	private float lastLevelEnd = 0;

	private LevelPiece lastObstacle;

	private void Awake()
	{
		//Set last obstacle
		lastObstacle = startGamePiece;
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

			// if (Random.Range(0f, 1f) <= bonusPrefabsSpawnChance)
			// {
			// 	SpawnBonus();
			// }
		}
		return SpawnEnd();
	}

	private LevelEnd SpawnEnd()
	{
		Vector3 point = new Vector3(0, 0, totalDistance);

		//Spawn the end part
		GameObject spawnedEnd = SpawnPiece(levelEndPrefab);

		//Get the trigger point on the end
		LevelEnd levelEnd = spawnedEnd.GetComponent<LevelEnd>();
		Transform triggerPoint = levelEnd.triggerPoint;

		//Call OnFinishedGeneratingLevel event
		if (OnFinishedGeneratingLevel != null) OnFinishedGeneratingLevel(lastLevelEnd, triggerPoint.position.z);

		return levelEnd;
	}

	private void SpawnObstacle()
	{
		LevelPiece chosenObstacle = ChooseObstacle();

		GameObject instance = SpawnPiece(chosenObstacle);


		_totalDistance += chosenObstacle.length;
	}
	private GameObject SpawnPiece(LevelPiece piece)
	{
		GameObject prefab = piece.gameObject;

		Vector3 offset = piece.transform.position - piece.start.position;
		Vector3 spawnPoint = lastObstacle.end.position + offset;

		Quaternion direction = Quaternion.identity; //lastObstacle.end.rotation;

		GameObject instance = Instantiate(prefab, spawnPoint, direction, pieceParent);

		lastObstacle = instance.GetComponent<LevelPiece>();

		return instance;
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

		//Create copy of obstacles array as a list
		List<LevelPiece> availableObstacles = new List<LevelPiece>();
		for (int i = 0; i < obstaclePrefabs.Length; i++)
		{
			availableObstacles.Add(obstaclePrefabs[i]);
		}
		Debug.Log("Available length: " + availableObstacles.Count);

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

		Debug.Log("Max Chosen length: " + availableObstacles[availableObstacles.Count - 1]);


		//Return chosen by index
		return availableObstacles[index];
	}
}
