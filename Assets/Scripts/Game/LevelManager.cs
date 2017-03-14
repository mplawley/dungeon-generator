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
	//Topology and 2DArrays to meet specifics for a continous journey through the dungeon
	public bool[,] connectedRoomsArray;
	public List<Room> connectedRooms = new List<Room>();
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
		connectedRoomsArray = new bool[DungeonColsInRooms, DungeonRowsInRooms];
		roomsArray = new Room[DungeonColsInRooms, DungeonRowsInRooms]; //Used to see if we have a straight journey through the Matrix-dungeon

		//InitializeConnectedRoomsArray();
		//InitializeMatrixPathfindingArray();

		//Place rooms....
		PlaceRooms ();

		//Ensure continuous path....

		//Remove orphaned rooms
		RemoveOrphanedRooms();

		//Place the Dungeon Holder beneath the Player so that he is centered....
		PlaceDungeon();

		//Check for a continuous journey and update....
		//MatrixContinuousPathCheck();
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
				if (i == middleCol && j == middleRow)
				{
					//Set the first room's connections to true.
					connectedRoomsArray[middleCol, middleRow] = true;
				}

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
						juncture = Instantiate (juncturePrefab, new Vector3 (i * roomWidth, j * roomHeight + roomHeight / 2 + architectureOffset, 0), Quaternion.identity) as GameObject; //Adding goes up world space
						correspondingJuncture = Instantiate (juncturePrefab, new Vector3 (i * roomWidth, (j - 1) * roomHeight - roomHeight / 2 - architectureOffset), Quaternion.Euler (0, 0, 180)) as GameObject;

						//Update connections
						connectedRoomsArray [i, j] = true;
						connectedRoomsArray [i, j - 1] = true; //Room to the north must be connected. Subtracting goes up the matrix, adding goes down.
					}
					else //Place an exit since we are at an outer wall...
					{
						juncture = Instantiate (exitPrefab, new Vector3 (i * roomWidth, j * roomHeight + roomHeight / 2 + architectureOffset, 0), Quaternion.identity) as GameObject;
					}
					break;
				case Compass.East:
					if(!easternmost)
					{
						juncture = Instantiate (juncturePrefab, new Vector3 (i * roomWidth + roomWidth / 2 + architectureOffset, j * roomHeight, 0), Quaternion.Euler (0, 0, 90)) as GameObject; //Adding goes right on world space
						correspondingJuncture = Instantiate (juncturePrefab, new Vector3 ((i + 1) * roomWidth - roomWidth / 2 - architectureOffset, j * roomHeight), Quaternion.Euler (0, 0, -90)) as GameObject;

						//Update connections
						connectedRoomsArray [i, j] = true;
						connectedRoomsArray [i + 1, j] = true; //Room to the east must be connected. Adding goes east in the matrix, subtracting goes west.
					}
					else //Place an exit since we are at an outer wall....
					{
						juncture = Instantiate (exitPrefab, new Vector3 (i * roomWidth + roomWidth / 2 + architectureOffset, j * roomHeight, 0), Quaternion.Euler (0, 0, 90)) as GameObject; 
					}
					break;
				case Compass.South:
					if(!southernmost)
					{
						juncture = Instantiate (juncturePrefab, new Vector3 (i * roomWidth, j * roomHeight - roomHeight / 2 - architectureOffset, 0), Quaternion.Euler (0, 0, 180)) as GameObject; //Subtracrting goes south on world space
						correspondingJuncture = Instantiate (juncturePrefab, new Vector3 (i * roomWidth, (j + 1) * roomHeight + roomHeight / 2 + architectureOffset), Quaternion.identity) as GameObject;

						//Update connections
						connectedRoomsArray [i, j] = true;
						connectedRoomsArray [i, j + 1] = true; //room to the south must be connected. Subtracting goes up the matrix, adding down.
					}
					else //Place an exit since we are at an outer wall...
					{
						juncture = Instantiate(exitPrefab, new Vector3(i * roomWidth, j * roomHeight - roomHeight / 2 - architectureOffset, 0), Quaternion.Euler(0,0,180)) as GameObject; 
					}
					break;
				case Compass.West:
					if(!westernmost)
					{
						juncture = Instantiate (juncturePrefab, new Vector3 (i * roomWidth - roomWidth / 2 - architectureOffset, j * roomHeight, 0), Quaternion.Euler (0, 0, -90)) as GameObject; //Subtracting goes left in world space
						correspondingJuncture = Instantiate (juncturePrefab, new Vector3 ((i - 1) * roomWidth + roomWidth / 2 + architectureOffset, j * roomHeight), Quaternion.Euler (0, 0, 90)) as GameObject;
					
						//Update connections
						connectedRoomsArray [i, j] = true;
						connectedRoomsArray [i - 1, j] = true; //Room to the west must be connected. Adding goes east in the matrix, subtracting goes west.
					}
					else //Place an exit since we are at an outer wall....
					{
						juncture = Instantiate(exitPrefab, new Vector3(i * roomWidth - roomWidth/2 - architectureOffset, j * roomHeight, 0), Quaternion.Euler(0,0,-90)) as GameObject;
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

	//Initializes bookkeeping array for what rooms have hallway junctures
	public void InitializeMatrixPathfindingArray ()
	{
		for (int i = 0; i < roomsArray.GetLength(0); i++)
		{
			for (int j = 0; j < roomsArray.GetLength(1); j++)
			{
//				roomsArray[i, j].SetNorth = false;
//				roomsArray[i, j].SetSouth = false;
//				roomsArray[i, j].SetWest = false;
//				roomsArray[i, j].SetEast = false;
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

	//Place player in the middle of the dungeon....
	public void PlaceDungeon()
	{
		Vector3 adjustedDungeonPosition = new Vector3((float)(dungeonHolder.transform.position.x - DungeonColsInRooms/2 * roomWidth), 
			(float)(dungeonHolder.transform.position.y - DungeonRowsInRooms/2 * roomHeight), 
			0f);
		
		dungeonHolder.transform.position = adjustedDungeonPosition;
	}

	//Check for a continuous journey....
//	public void MatrixContinuousPathCheck()
//	{
//		//Look at where the middle of the matrix is because that is where the player starts....
//		int middleCol = DungeonColsInRooms / 2;
//		int middleRow = DungeonRowsInRooms / 2;
//
//		int currentCol = middleCol;
//		int currentRow = middleRow;
//		int continuousRoomCount = 0;
//
//		//Check that there are continuous connections based on the Inspector variables...if not, make them...
//		for (int i = 0; i < minRoomsBetweenEnterAndExit; i++)
//		{
//			//Check north....
//			if (roomsArray[currentCol, currentRow].northDoor)
//			{
//				currentRow += 1;
//				continuousRoomCount++;
//			}
//			else if (roomsArray[currentCol, currentRow].eastDoor)
//			{
//				currentCol += 1;
//				continuousRoomCount++;
//			}
//			else if (roomsArray[currentCol, currentRow].southDoor)
//			{
//				currentRow -= 1;
//				continuousRoomCount++;
//			}
//			else if (roomsArray[currentCol, currentRow].westDoor)
//			{
//				currentCol -= 1;
//				continuousRoomCount++;
//			}
//			else
//			{
//				//If we have satisfied a path through the dungeon that is at least as long (in rooms) as specified....
//				if (continuousRoomCount >= minRoomsBetweenEnterAndExit)
//				{
//					//Place an exit out of the dungeon in this room....
//					GameObject exit = Instantiate(exitPrefab, new Vector3(currentCol * roomWidth + roomWidth / 2 + architectureOffset, currentRow * roomHeight, 0), Quaternion.identity) as GameObject;
//
//					//Shut it down: base case...
//					return;
//				}
//				else
//				{
//					//TODO: HIERARCHICALLY DECOMPOSE REPEATED CODE INTO A FUNCTION!
//					//Generate another door and call this function again....
//				}
//			}
//		}
//	}
}
