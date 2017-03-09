using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
	public enum Compass {North, East, South, West};

	[Header("Overall architecture")]
	[SerializeField]
	int DungeonRowsInRooms;

	[SerializeField]
	int DungeonColsInRooms;

	[SerializeField]
	int minRoomsBetweenEnterAndExit;

	[Header("Room specifications")]
	[SerializeField]
	int roomWidth;

	[SerializeField]
	int roomHeight;

	[Header("Prefabs")]
	public GameObject roomPrefab;
	public GameObject juncturePrefab;
	public GameObject solidWallPrefab;

	[Header("Housekeeping")]
	public bool[,] connectedRoomsArray;
	public GameObject[,] gameObjectArray;
	public Transform architectureParent;
	public Transform junctureParent;
	public Transform solidWallParent;
	public float architectureOffset; //For visual pleasure of placement

	// Use this for initialization
	void Start()
	{
		//Bookkeeping for which rooms have hallway junctures connecting them...
		connectedRoomsArray = new bool[DungeonColsInRooms, DungeonRowsInRooms];
		gameObjectArray = new GameObject[DungeonColsInRooms,DungeonRowsInRooms];
		InitializeConnectedRoomsArray();

		//Place rooms....
		PlaceRooms ();

		//Remove orphaned rooms
		RemoveOrphanedRooms();
	}

	void PlaceRooms()
	{
		//Bookkeeping for perimeter rooms of the dungeon....
		bool northernmost = false;
		bool westernmost = false;
		bool easternmost = false;
		bool southernmost = false;

		//Place rooms
		for (int i = 0; i < DungeonColsInRooms; i++)
		{
			for (int j = 0; j < DungeonRowsInRooms; j++)
			{
				//Check if this room is an outermost one in the dungeon....
				//Reset bools
				northernmost = false;
				westernmost = false;
				easternmost = false;
				southernmost = false;

				//Check bools
				if (i == 0)
				{
					northernmost = true;
				}
				if (j == 0)
				{
					westernmost = true;
				}
				if (i == DungeonColsInRooms - 1)
				{
					southernmost = true;
				}
				if (j == DungeonRowsInRooms - 1)
				{
					easternmost = true;
				}

				//Otherwise, this is an interior room. Generate junctures to other rooms....
				Compass direction = (Compass)Random.Range(0,4);
				GameObject juncture;

				switch (direction)
				{
				case Compass.North:
					if (!northernmost)
					{
						juncture = Instantiate(juncturePrefab, new Vector3(i * roomHeight - roomHeight/2 + architectureOffset, j * roomWidth, 0), Quaternion.identity) as GameObject;

						//Set juncture parent transform...
						juncture.transform.parent = junctureParent;

						//Update connections
						connectedRoomsArray[i, j] = true;
						connectedRoomsArray[i-1, j] = true; //Room to the south must be connected
					}
					break;
				case Compass.East:
					if (!easternmost)
					{
						juncture = Instantiate(juncturePrefab, new Vector3(i * roomHeight, j * roomWidth + roomWidth/2 + architectureOffset, 0), Quaternion.Euler(0,0,90)) as GameObject;

						//Set juncture parent transform...
						juncture.transform.parent = junctureParent;

						//Update connections
						connectedRoomsArray[i, j] = true;
						connectedRoomsArray[i, j+1] = true; //Room to the east must be connected
					}
					break;
				case Compass.South:
					if (!southernmost)
					{
						juncture = Instantiate(juncturePrefab, new Vector3(i * roomHeight + roomHeight/2 + architectureOffset, j * roomWidth, 0), Quaternion.identity) as GameObject;

						//Set juncture parent transform...
						juncture.transform.parent = junctureParent;

						//Update connections
						connectedRoomsArray[i, j] = true;
						connectedRoomsArray[i+1, j] = true; //room to the north must be connected
					}
					break;
				case Compass.West:
					if (!westernmost)
					{
						juncture = Instantiate(juncturePrefab, new Vector3(i * roomHeight, j * roomWidth - roomWidth/2 + architectureOffset, 0), Quaternion.Euler(0,0,90)) as GameObject;

						//Set juncture parent transform...
						juncture.transform.parent = junctureParent;

						//Update connections
						connectedRoomsArray[i, j] = true;
						connectedRoomsArray[i, j-1] = true; //Room to the west must be connected
					}
					break;
				default:
					break;
				}
					
				//Finally, place the room
				GameObject room = Instantiate(roomPrefab, new Vector3(i * roomHeight, j * roomWidth, 0), Quaternion.identity) as GameObject;
				room.transform.parent = architectureParent;
			}
		}
	}


	//Initializes bookkeeping array for what rooms have hallway junctures
	public void InitializeConnectedRoomsArray ()
	{
		for (int i = 0; i < connectedRoomsArray.GetLength(0); i++)
		{
			for (int j = 0; j < connectedRoomsArray.GetLength(1); j++)
			{
				connectedRoomsArray [i, j] = false;
			}
		}
	}

	//Cut out orphan rooms....
	public void RemoveOrphanedRooms()
	{
		for (int i = 0; i < connectedRoomsArray.GetLength(0); i++)
		{
			for (int j = 0; j < connectedRoomsArray.GetLength (1); j++)
			{
				if (connectedRoomsArray [i, j] == false)
				{
					//TODO: FIX
					//GameObject orphanedRoom = gameObjectArray [i, j];
					//Destroy (orphanedRoom);

					GameObject solidWall = Instantiate(solidWallPrefab, new Vector3(i * roomWidth, j * roomHeight, 0), Quaternion.identity) as GameObject;
					solidWall.transform.parent = solidWallParent;
				}
			}
		}
	}
}
