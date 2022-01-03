using UnityEngine;

[RequireComponent(typeof(LevelGenerator))]
public class SeedRandomizer : MonoBehaviour
{
	// Start is called before the first frame update
	void Awake()
	{
		GetComponent<LevelGenerator>().seed = Random.Range(0, 10000000);
	}
}
