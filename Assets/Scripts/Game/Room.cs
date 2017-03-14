using UnityEngine;

public class Room //DOES NOT inherit from MonoBehaviour
{
	//Doors
	public bool northDoor;
	public bool southDoor;
	public bool westDoor;
	public bool eastDoor;

	//Exits
	public bool northExit;
	public bool southExit;
	public bool westExit;
	public bool eastExit;

	//Bookmarked
	public bool thisRoomCrawled;

	//Constructor
	public Room()
	{
		northDoor = false;
		southDoor = false;
		westDoor = false;
		eastDoor = false;

		northExit = false;
		southExit = false;
		westExit = false;
		eastExit = false;

		thisRoomCrawled = false;
	}
}
