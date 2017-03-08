using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
	//Singleton
	public static MonsterManager instance;

	//Boundaries for monster movement
	public float xMin, xMax, yMin, yMax;

	void Awake()
	{
		//Singleton
		if (instance != this)
		{
			instance = this;
		} 
		else
		{
			Destroy(this);
		}
	}

	//Spawn function

	//Enemy life 

}
