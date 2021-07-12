using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleGenerator : MonoBehaviour
{
	[Header("Main settings")]
	public float startDistance = 30;
	public float spawnDistance = 10;
	public int obstaclesToSpawn = 20;
	public float groundEndSpace = 10;
	public float groundStartSpace = 5;
	[Range(0f, 1f)]
	public float funStuffSpawnChance = 0.2f;

	[Header("Level Settings")]
	public float obstacleIncreasePerLevel = 2;

	public GameObject[] obstacles;
	public GameObject[] funStuff;
	[System.Serializable]
	public struct BadCombo
	{
		public GameObject part1;
		public GameObject part2;
	}
	public BadCombo pairToAvoid;
	public GameObject end;
	public GameObject player;
	public GameObject[] winStuff;

	public Transform groundTransform;
	public Renderer groundRenderer;
	void Start()
	{
		Settings settings = (GameObject.Find("Settings") != null) ? GameObject.Find("Settings").GetComponent<Settings>() : null;
		if (settings != null)
		{
			obstaclesToSpawn += (int)(obstacleIncreasePerLevel * settings.level);
		}
		//set the space behind the player
		groundTransform.localScale = new Vector3(1, 1, groundStartSpace);
		//make it so that the ground ends at z = 0
		groundTransform.position = new Vector3(0, 0, -groundStartSpace / 2);
		//add more tiling the texture to match scale increase
		groundRenderer.material.mainTextureScale += new Vector2(0, groundStartSpace);


		//move it forward half its size increase
		groundTransform.localScale += new Vector3(0, 0, startDistance);
		//make it so that the ground ends at z = 0
		groundTransform.position += new Vector3(0, 0, startDistance / 2);
		//add more tiling the texture to match scale increase
		groundRenderer.material.mainTextureScale += new Vector2(0, spawnDistance);

		lastObstacle = Random.Range(0, obstacles.Length - 1);
		SpawnObstacle();
	}

	// Update is called once per frame
	int spawnedObstacles = 0;
	int lastObstacle;
	bool done = false;
	void Update()
	{
		if (spawnedObstacles < obstaclesToSpawn) SpawnObstacle();
		else if (!done)
		{
			SpawnEnd();
		}
	}
	void SpawnEnd()
	{
		done = true;
		GameObject endSpawn = Instantiate(end, new Vector3(0, 0, startDistance + spawnDistance * (spawnedObstacles + 1)), Quaternion.identity, transform);

		foreach (TripWire trip in endSpawn.GetComponents<TripWire>())
		{
			trip.player = player;
		}
		player.GetComponent<WinLose>().winGate = endSpawn.transform;

	}
	int getIndex(int top, int exception)
	{
		int num = Random.Range(0, top - 1);
		return (num >= exception) ? num + 1 : num;
	}
	void SpawnObstacle()
	{
		spawnedObstacles++;

		//move it forward half its size increase
		groundTransform.localScale += new Vector3(0, 0, spawnDistance);
		//make it so that the ground ends at z = 0
		groundTransform.position += new Vector3(0, 0, spawnDistance / 2);
		//add more tiling the texture to match scale increase
		groundRenderer.material.mainTextureScale += new Vector2(0, spawnDistance);
		int index = 0;
		if (obstacles[lastObstacle] == pairToAvoid.part1)
		{
			for (int i = 0; i < obstacles.Length; i++)
			{
				if (obstacles[i] == pairToAvoid.part2)
					index = getIndex(obstacles.Length - 1, i);
			}

		}
		else if (obstacles[lastObstacle] == pairToAvoid.part2)
		{
			for (int i = 0; i < obstacles.Length; i++)
			{
				if (obstacles[i] == pairToAvoid.part1)
					index = getIndex(obstacles.Length - 1, i);
			}

		}
		else
			index = getIndex(obstacles.Length - 1, lastObstacle);



		GameObject spawnObj = Instantiate(obstacles[index], new Vector3(0, 0, startDistance + spawnDistance * spawnedObstacles), Quaternion.identity, transform);
		lastObstacle = index;

		foreach (TripWire trip in spawnObj.GetComponents<TripWire>())
		{
			trip.player = player;
		}

		if (Random.Range(0f, 1f) <= funStuffSpawnChance)
			Instantiate(funStuff[(int)Mathf.Round(Random.Range(0f, funStuff.Length - 1f))], new Vector3(Random.Range(-4, 4), 0, startDistance + spawnDistance * spawnedObstacles - 1), Quaternion.identity, transform);
	}
}
