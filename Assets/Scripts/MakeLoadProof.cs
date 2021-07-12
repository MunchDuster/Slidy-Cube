using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeLoadProof : MonoBehaviour
{
	private void Awake()
	{
		DontDestroyOnLoad(this.gameObject);
	}
}
