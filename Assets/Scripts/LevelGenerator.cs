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

	[System.Serializable]
	public class SideThingy
	{
		public string name;

		public GameObject[] gameObjects;

		[Space(10)]
		public float lifetime = 10f;

		[Space(10)]
		public float maxY = 20f;
		public float minY = 20f;

		[Space(10)]
		public float minScale = 1f;
		public float maxScale = 1.5f;
	}

	public List<Level> levels = new List<Level>();
	[Header("Main settings")]
	public int seed = 10994394;

	[Header("Chances")]
	public int obstaclePrefabsToSpawn = 20;
	[Range(0f, 1f)] public float bonusPrefabsSpawnChance = 0.2f;
	[Range(0f, 1f)] public float sideThingSpawnChance = 0.8f;

	public float obstacleIncreasePerLevel = 2;

	[Space(10)]
	public float levelDestroyTime = 3;

	[Header("Inner Refs")]
	public LevelPiece startGamePiece;
	public LevelPiece levelEndPrefab;

	[Space(10)]

	public LevelPiece[] obstaclePrefabs;
	public GameObject[] bonusPrefabs;
	public SideThingy[] sideThings;

	public BadCombo[] badCombos;

	[Header("Outer Refs")]
	public Transform pieceParent;
	public Transform player;

	[HideInInspector] public float lowestPointY = -1;

	public delegate void OnLevelEvent(float start, float end);
	public OnLevelEvent OnFinishedGeneratingLevel;

	private float _totalDistance = 0;
	private float totalDistance { get { return _totalDistance; } }
	private float lastLevelEnd = 0;

	private LevelPiece lastObstacle;
	private System.Random randomNumberGenerator;

	private void Start()
	{
		//Set last obstacle
		lastObstacle = startGamePiece;

		randomNumberGenerator = new System.Random(seed);
	}

	public Level GenerateLevel()
	{
		lastLevelEnd = totalDistance;

		obstaclePrefabsToSpawn += (int)(obstacleIncreasePerLevel * Settings.level);

		float startDistance = startGamePiece.length;
		_totalDistance += startDistance;

		LevelPiece[] pieces = new LevelPiece[obstaclePrefabsToSpawn];

		for (int i = 0; i < obstaclePrefabsToSpawn; i++)
		{
			pieces[i] = SpawnObstacle();

			// if (Random.Range(0f, 1f) <= bonusPrefabsSpawnChance) SpawnBonus();

			// if (randomNumberGenerator.Next(0, 1000) <= sideThingSpawnChance * 1000f) SpawnSideThing();
		}

		LevelEnd levelEnd = SpawnEnd();

		return new Level(this, pieces, levelEnd, levelDestroyTime);
	}

	private LevelEnd SpawnEnd()
	{
		Vector3 point = new Vector3(0, 0, totalDistance);

		//Spawn the end part
		LevelPiece spawnedEnd = SpawnPiece(levelEndPrefab);

		//Get the trigger point on the end
		LevelEnd levelEnd = spawnedEnd.GetComponent<LevelEnd>();
		Transform triggerPoint = levelEnd.triggerPoint;

		//Call OnFinishedGeneratingLevel event
		if (OnFinishedGeneratingLevel != null) OnFinishedGeneratingLevel(lastLevelEnd, triggerPoint.position.z);

		return levelEnd;
	}

	private LevelPiece SpawnObstacle()
	{
		LevelPiece chosenObstacle = ChooseObstacle();

		LevelPiece instance = SpawnPiece(chosenObstacle);

		_totalDistance += chosenObstacle.length;

		return instance;
	}
	private LevelPiece SpawnPiece(LevelPiece piece)
	{
		GameObject prefab = piece.gameObject;

		Vector3 offset = piece.transform.position - piece.start.position;
		Vector3 spawnPoint = lastObstacle.end.position + offset;

		Quaternion offsetRot = piece.transform.rotation * piece.start.rotation;
		Quaternion spawnRotation = offsetRot * lastObstacle.end.rotation;

		GameObject instance = Instantiate(prefab, spawnPoint, spawnRotation, pieceParent);

		LevelPiece instancePiece = instance.GetComponent<LevelPiece>();

		instancePiece.lastPiece = lastObstacle;

		lastObstacle = instancePiece;

		Destroy(instance, 30);

		float thisLowestPointY = Mathf.Min(instancePiece.start.position.y, instancePiece.end.position.y);

		if (thisLowestPointY < lowestPointY)
		{
			lowestPointY = thisLowestPointY;
		}

		return instancePiece;
	}

	private void SpawnSideThing()
	{
		int index = randomNumberGenerator.Next(0, sideThings.Length);
		SideThingy sideThingy = sideThings[index];

		float x = (Random.value > 0.5f) ? Random.Range(30f, 60f) : -Random.Range(20f, 30f);
		float y = Random.Range(sideThingy.minY, sideThingy.maxY);

		float z = player.position.z + Random.Range(40f, 80f);


		float scale = Random.Range(sideThingy.minScale, sideThingy.maxScale);

		Vector3 offset = new Vector3(x, y, z);
		Vector3 pos = lastObstacle.end.position + offset;

		int gameObjectIndex = randomNumberGenerator.Next(0, sideThingy.gameObjects.Length);

		GameObject instance = Instantiate(sideThingy.gameObjects[gameObjectIndex], pos, Quaternion.identity);
		instance.transform.localScale = Vector3.one * scale;

		Destroy(instance, 15);
	}

	private void SpawnBonus()
	{
		GameObject prefab = bonusPrefabs[(int)Mathf.Round(randomNumberGenerator.Next(0, bonusPrefabs.Length))];
		Vector3 position = new Vector3(randomNumberGenerator.Next(-4, 4), 0, totalDistance);

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

		//Remove last obstacle
		availableObstacles.Remove(lastObstacle);

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
		int index = randomNumberGenerator.Next(0, availableObstacles.Count);

		//Return chosen by index
		return availableObstacles[index];
	}
}
