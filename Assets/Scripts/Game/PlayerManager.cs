using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

	// Use this for initialization
	void Start()
	{
		print("starting");
		MemoryManager.instance.Load();
	}
	
	// Update is called once per frame
	void Update()
	{
		
	}
}
