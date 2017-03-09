/*
 * This script generates the dungeon as a 2D array of ints.
 * 0 is solid stone.
 * 1 is a normal room.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
	[Header("Procedural generation fields")]
	int dungeonWidth = 10;
	int dungeonHeight = 10;
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
		//Generate the dungeon board as a 2D array....(Dimensions * 2) -1 ensures we will always have enough room for the entire dungeon
		int maxDungeonWidth = (dungeonWidth * 2) - 1; 
		int maxDungeonHeight = (dungeonHeight * 2) - 1;
		int[,] dungeonArray2D = new int[maxDungeonHeight, maxDungeonWidth];

		//Place the first room right in its middle...
		dungeonArray2D[maxDungeonHeight/2, maxDungeonWidth/2] = 1;

		//Output the dungeon

		for (int i = 0; i < maxDungeonHeight; i++)
		{
			for (int j = 0; j < maxDungeonWidth; j++)
			{
				Debug.Log(string.Format("{0} ", dungeonArray2D[i, j]));
			}
			Debug.Log("\n" + "\n");
		}
	}

	void GenerateDungeon()
	{
		for (int i = 0; i < maxRoomWidth; i++)
		{
			//	
		}
	}
}
