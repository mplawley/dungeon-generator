/*
 * This script employs a matrix/2D Array approach to procedural dungeon generation
 * TODO: add more room prefabs and unique tile prefabs for gameplay and visual variety!
 */

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
	public GameObject playerPrefab;
	public GameObject roomPrefab; //TODO: delete after testing--no longer needed with the List implementaiton of roomPrefabs....
	public GameObject juncturePrefab;
	public GameObject solidWallPrefab;
	public GameObject dungeonHolder;
	public GameObject exitPrefab;
	public List<GameObject> roomPrefabs = new List<GameObject>();

	[Header("Housekeeping")]
	//2DArray to meet specifics for a continous journey through the dungeon
	public Room[,] roomsArray;

	//Visual presentation
	public Transform architectureParent;
	public Transform junctureParent;
	public Transform solidWallParent;
	public float architectureOffset; //For visual pleasure of placement

	// Use this for initialization
	void Start()
	{
		//Bookkeeping for which rooms have hallway junctures connecting them...
		roomsArray = new Room[DungeonColsInRooms, DungeonRowsInRooms]; //Used to see if we have a straight journey through the Matrix-dungeon
		InitializeRoomsArray ();

		//Place rooms....
		PlaceRooms ();

		//Ensure continuous path....
		int i = DungeonColsInRooms/2;
		int j = DungeonRowsInRooms/2;
		int numberOfRoomsInPath = 0;
		EnsureContinuousPath(i, j, numberOfRoomsInPath);

		//Remove orphaned rooms
		RemoveOrphanedRooms();

		//Place the Dungeon Holder beneath the Player so that he is centered....
		PlaceDungeon();
	}

	//Initializes the Rooms array so that there are no null values in the Rooms matrix
	void InitializeRoomsArray()
	{
		for (int i = 0; i < roomsArray.GetLength (0); i++)
		{
			for (int j = 0; j < roomsArray.GetLength (1); j++)
			{
				roomsArray [i, j] = new Room ();
			}
		}
	}

	//This function places the rooms iteratively onto the game board
	void PlaceRooms()
	{
		//Bookkeeping for perimeter rooms of the dungeon
		bool northernmost = false;
		bool westernmost = false;
		bool easternmost = false;
		bool southernmost = false;

		//Place rooms by iterating through the dungeon matrix and translating position within the 2D array into world space dungeon prefab placements...
		for (int i = 0; i < DungeonColsInRooms; i++)
		{
			for (int j = 0; j < DungeonRowsInRooms; j++)
			{
				//Check if this is the FIRST ROOM of the player's journey, which is in the middle of the matrix....
				int middleCol = DungeonColsInRooms / 2;
				int middleRow = DungeonRowsInRooms / 2;

				//Check if this room is an outermost one in the dungeon....if it is, don't generate a junture to another part of the matrix....
				//Reset bools
				northernmost = false;
				westernmost = false;
				easternmost = false;
				southernmost = false;

				//Check those bools
				if (i == 0)
				{
					westernmost = true;
				}
				if (j == 0)
				{
					northernmost = true;
				}
				if (i == DungeonColsInRooms - 1)
				{
					easternmost = true;
				}
				if (j == DungeonRowsInRooms - 1)
				{
					southernmost = true;
				}

				//If we aren't on the perimeter, then we are currently placing an interior room. Generate junctures to other rooms in the matrix....
				Compass direction = (Compass)Random.Range(0,4);

				//Initialize juncture and correspondingJuncture to any gameObject before the switch statement assigns them correctly...
				GameObject juncture = this.gameObject;
				GameObject correspondingJuncture = this.gameObject; //To connect rooms. E.g., if a room has a north door, the room above it must have a south door....

				switch (direction)
				{
				case Compass.North:
					if(!northernmost)
					{
						juncture = Instantiate (juncturePrefab, new Vector3 (i * roomWidth, j * roomHeight - roomHeight / 2 - architectureOffset, 0), Quaternion.identity) as GameObject; //Subtracting goes UP world space, now oriented like the matrix

						if(!southernmost)
						{
							correspondingJuncture = Instantiate (juncturePrefab, new Vector3 (i * roomWidth, (j - 1) * roomHeight + roomHeight / 2 + architectureOffset), Quaternion.Euler (0, 0, 180)) as GameObject;
						}

						//Update connections
						roomsArray[i,j].northDoor = true;
						roomsArray[i,j-1].southDoor = true; //Room to the north must be connected. Subtracting goes up the matrix, adding goes down.
					}
					else //Place an exit since we are at an outer wall...
					{
						juncture = Instantiate (exitPrefab, new Vector3 (i * roomWidth, j * roomHeight - roomHeight / 2 - architectureOffset, 0), Quaternion.identity) as GameObject;
						roomsArray [i, j].northExit = true;
					}
					break;
				case Compass.East:
					if(!easternmost)
					{
						juncture = Instantiate (juncturePrefab, new Vector3 (i * roomWidth + roomWidth / 2 + architectureOffset, j * roomHeight, 0), Quaternion.Euler (0, 0, 90)) as GameObject; //Adding goes right on world space

						if(!westernmost)
						{
							correspondingJuncture = Instantiate (juncturePrefab, new Vector3 ((i + 1) * roomWidth - roomWidth / 2 - architectureOffset, j * roomHeight), Quaternion.Euler (0, 0, -90)) as GameObject;
						}

						//Update connections
						roomsArray[i,j].eastDoor = true;
						roomsArray [i+1, j].westDoor = true; //Room to the east must be connected. Adding goes east in the matrix, subtracting goes west. 
					}
					else //Place an exit since we are at an outer wall....
					{
						juncture = Instantiate (exitPrefab, new Vector3 (i * roomWidth + roomWidth / 2 + architectureOffset, j * roomHeight, 0), Quaternion.Euler (0, 0, 90)) as GameObject; 
						roomsArray [i, j].eastExit = true;
					}
					break;
				case Compass.South:
					if(!southernmost)
					{
						juncture = Instantiate (juncturePrefab, new Vector3 (i * roomWidth, j * roomHeight + roomHeight / 2 + architectureOffset, 0), Quaternion.Euler (0, 0, 180)) as GameObject; //Adding goes down on world space, oriented like the matrix

						if(!northernmost)
						{
							correspondingJuncture = Instantiate (juncturePrefab, new Vector3 (i * roomWidth, (j + 1) * roomHeight - roomHeight / 2 - architectureOffset), Quaternion.identity) as GameObject;
						}

						//Update connections
						roomsArray[i,j].southDoor = true;
						roomsArray[i,j+1].northDoor = true; //room to the south must be connected. Subtracting goes up the matrix, adding down.
					}
					else //Place an exit since we are at an outer wall...
					{
						juncture = Instantiate(exitPrefab, new Vector3(i * roomWidth, j * roomHeight + roomHeight / 2 + architectureOffset, 0), Quaternion.Euler(0,0,180)) as GameObject; 
						roomsArray [i, j].southExit = true;
					}
					break;
				case Compass.West:
					if(!westernmost)
					{
						juncture = Instantiate (juncturePrefab, new Vector3 (i * roomWidth - roomWidth / 2 - architectureOffset, j * roomHeight, 0), Quaternion.Euler (0, 0, -90)) as GameObject; //Subtracting goes left in world space

						if(!easternmost)
						{
							correspondingJuncture = Instantiate (juncturePrefab, new Vector3 ((i - 1) * roomWidth + roomWidth / 2 + architectureOffset, j * roomHeight), Quaternion.Euler (0, 0, 90)) as GameObject;
						}
					
						//Update connections
						roomsArray[i,j].westDoor = true;
						roomsArray[i-1,j].eastDoor = true; //Room to the west must be connected. Adding goes east in the matrix, subtracting goes west.
					}
					else //Place an exit since we are at an outer wall....
					{
						juncture = Instantiate(exitPrefab, new Vector3(i * roomWidth - roomWidth/2 - architectureOffset, j * roomHeight, 0), Quaternion.Euler(0,0,-90)) as GameObject;
						roomsArray [i, j].westExit = true;
					}
					break;
				default:
					break;
				}

				//Set juncture parent transform...
				juncture.transform.parent = junctureParent;
				correspondingJuncture.transform.parent = junctureParent;
					
				//Finally, place the room itself AFTER the above junctures are placed....
				int randomRoomNum = Random.Range(0, roomPrefabs.Count);
				GameObject room = Instantiate(roomPrefabs[randomRoomNum], new Vector3(i * roomHeight, j * roomWidth, 0), Quaternion.identity) as GameObject;
				room.transform.parent = architectureParent;
			}
		}
	}

	//This ensures a continuous path between the player's start position and a dungeon exit....
	public void EnsureContinuousPath(int i, int j, int numberOfRoomsInPath)
	{
		//Base case 1: If we hit the edge of the map, make an exit and return....
		if (i >= DungeonColsInRooms-1 || j >= DungeonRowsInRooms-1 || i <= 0 || j <= 0)
		{
			GameObject newDoor = Instantiate (juncturePrefab, new Vector3 (i * roomWidth + roomWidth / 2 + architectureOffset, j * roomHeight, 0), Quaternion.Euler (0, 0, 90)) as GameObject;
			newDoor.transform.parent = junctureParent;
			return;
		}

		//Base case 2: If we found an exit, return....
		if (roomsArray [i, j].northExit || roomsArray [i, j].eastExit || roomsArray [i, j].southExit || roomsArray [i, j].westExit)
		{
			return;
		}

		//Mark this room as crawled and increment now many rooms we have crawled so far....
		roomsArray [i, j].thisRoomCrawled = true;
		numberOfRoomsInPath += 1;

		//Follow paths populated by doors....
		//Check indexes carefully as we crawl....
		if (roomsArray [i, j].northDoor && !roomsArray [i, j - 1].thisRoomCrawled && (j - 1) >= 0)
		{
			j -= 1;
			EnsureContinuousPath (i, j, numberOfRoomsInPath);
		}
		else if (roomsArray [i, j].eastDoor && !roomsArray [i + 1, j].thisRoomCrawled && (i + 1) <= DungeonColsInRooms)
		{
			i += 1;
			EnsureContinuousPath (i, j, numberOfRoomsInPath);
		}
		else if (roomsArray [i, j].southDoor && !roomsArray [i, j + 1].thisRoomCrawled && (j+1) <= DungeonRowsInRooms)
		{
			j += 1;
			EnsureContinuousPath (i, j, numberOfRoomsInPath);
		}
		else if (roomsArray [i, j].westDoor && !roomsArray [i - 1, j].thisRoomCrawled && (i-1) >= 0)
		{
			i -= 1;
			EnsureContinuousPath (i, j, numberOfRoomsInPath);
		}

		//If we haven't recursively called self by now, time to make a door....
		if (roomsArray [i, j].eastDoor && (i-1) >= 0)
		{
			//Door to the west....
			GameObject newDoor = Instantiate (juncturePrefab, new Vector3 (i * roomWidth - roomWidth / 2 - architectureOffset, j * roomHeight, 0), Quaternion.Euler (0, 0, -90)) as GameObject;
			newDoor.transform.parent = junctureParent;

			//Go west...
			i -= 1;
			EnsureContinuousPath (i, j, numberOfRoomsInPath);
		}
		else if (roomsArray [i, j].westDoor && (i+1) <= DungeonColsInRooms)
		{
			//Exit to the east....
			GameObject newDoor = Instantiate (juncturePrefab, new Vector3 (i * roomWidth + roomWidth / 2 + architectureOffset, j * roomHeight, 0), Quaternion.Euler (0, 0, 90)) as GameObject;
			newDoor.transform.parent = junctureParent;

			//Go east
			i += 1;
			EnsureContinuousPath (i, j, numberOfRoomsInPath);
		}
		else if (roomsArray [i, j].northDoor && (j+1) <= DungeonRowsInRooms)
		{
			//Exit to the south....
			GameObject newDoor = Instantiate (juncturePrefab, new Vector3 (i * roomWidth, j * roomHeight + roomHeight / 2 + architectureOffset, 0), Quaternion.Euler (0, 0, 180)) as GameObject;
			newDoor.transform.parent = junctureParent;

			//Go south....
			j += 1;
			EnsureContinuousPath (i, j, numberOfRoomsInPath);
		}
		else if (roomsArray [i, j].southDoor && (j-1) >= 0)
		{
			//Exit to the north....
			GameObject newDoor = Instantiate (juncturePrefab, new Vector3 (i * roomWidth, j * roomHeight - roomHeight / 2 - architectureOffset, 0), Quaternion.identity) as GameObject;
			newDoor.transform.parent = junctureParent;

			//Go north...
			j -= 1;
			EnsureContinuousPath (i, j, numberOfRoomsInPath);
		}

		//If we still haven't called self by now, check against number of rooms required, make an exit if satisifed, and return....
		if (numberOfRoomsInPath >= minRoomsBetweenEnterAndExit)
		{
			//TODO: make an exit
			return;
		}
		else
		{
			//TODO: call self yet again
			return;
		}
	}
		
	//Cut out orphan rooms....
	public void RemoveOrphanedRooms()
	{
		for (int i = 0; i < roomsArray.GetLength(0); i++)
		{
			for (int j = 0; j < roomsArray.GetLength (1); j++)
			{
				if (!roomsArray [i, j].eastDoor && !roomsArray [i, j].eastExit && !roomsArray [i, j].westDoor && !roomsArray [i, j].westExit
				    && !roomsArray [i, j].northDoor && !roomsArray [i, j].northExit && !roomsArray [i, j].southDoor && !roomsArray [i, j].southExit)
				{
					GameObject solidWall = Instantiate(solidWallPrefab, new Vector3(i * roomWidth, j * roomHeight, 0), Quaternion.identity) as GameObject;
					solidWall.transform.parent = solidWallParent;
				}
			}
		}
	}

	//Place player in the middle of the dungeon....
	public void PlaceDungeon()
	{
		Vector3 adjustedDungeonPosition = new Vector3((float)(dungeonHolder.transform.position.x - DungeonColsInRooms/2 * roomWidth), 
			(float)(dungeonHolder.transform.position.y - DungeonRowsInRooms/2 * roomHeight), 
			0f);
		
		dungeonHolder.transform.position = adjustedDungeonPosition;
	}
}
