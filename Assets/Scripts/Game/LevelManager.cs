using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
	[Header("Procedural generation fields")]
	int maxRooms;
	int maxCorridors;
	int maxRoomWidth = 16;
	int maxRoomHeight = 8;

	[Header("General terrain")]
	[SerializeField]
	List<GameObject> generalRoomPrefabs = new List<GameObject>();

	[SerializeField]
	List<GameObject> specialRoomPrefabs = new List<GameObject>();

	[SerializeField]
	List<GameObject> groundTiles = new List<GameObject>();

	[SerializeField]
	List<GameObject> outerWalls = new List<GameObject>();

	[SerializeField]
	List<GameObject> trapTiles = new List<GameObject>();

	[Header("Doors")]
	List<GameObject> doorTiles = new List<GameObject>();



	// Use this for initialization
	void Start()
	{
		//Place the first room....
		PlaceFirstRoom();

		//GenerateDungeon();
	}

	void PlaceFirstRoom()
	{
		//Generate random number....
		int randomNumber = Random.Range(0, generalRoomPrefabs.Count);
	}

	void GenerateDungeon()
	{
		
		for (int i = 0; i < maxRoomWidth; i++)
		{
			//	
		}
	}
}
