using UnityEngine;

public class Room //DOES NOT inherit from MonoBehaviour
{
	public bool northDoor = false;
	public bool southDoor = false;
	public bool westDoor = false;
	public bool eastDoor = false;

	public bool firstRoom = false;
	public bool partOfAContinuousJourney = false;

	public bool SetNorth
	{
		get 
		{
			return northDoor;
		}
		set 
		{
			northDoor = value;
		}
	}

	public bool SetSouth
	{
		get 
		{
			return southDoor;
		}
		set 
		{
			southDoor = value;
		}
	}

	public bool SetWest
	{
		get 
		{
			return westDoor;
		}
		set 
		{
			westDoor = value;
		}
	}

	public bool SetEast
	{
		get 
		{
			return eastDoor;
		}
		set 
		{
			eastDoor = value;
		}
	}

	public void SetBooleans(bool northDoorStatus, bool southDoorStatus, bool westDoorStatus, bool eastDoorStatus, bool partOfAContinuousJourneyStatus, bool firstRoomStatus)
	{
		//Doors
		northDoor = northDoorStatus;
		southDoor = southDoorStatus;
		westDoor = westDoorStatus;
		eastDoor = eastDoorStatus;

		//Contiguity/rough Matrix topology
		firstRoom = firstRoomStatus;
		partOfAContinuousJourney = partOfAContinuousJourneyStatus; //TODO
	}
}
