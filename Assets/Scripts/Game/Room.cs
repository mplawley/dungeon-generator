using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Room //DOES NOT inherit from MonoBehaviour
{
	public bool northDoor = false;
	public bool southDoor = false;
	public bool westDoor = false;
	public bool eastDoor = false;

	public bool firstRoom = false;
	public bool partOfAContinuousJourney = false;

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

	public void SetNorthDoor(bool northDoorStatus)
	{
		northDoor = northDoorStatus;
	}

	public void SetSouthDoor(bool southDoorStatus)
	{
		southDoor = southDoorStatus;
	}

	public void SetWestDoor(bool westDoorStatus)
	{
		westDoor = westDoorStatus;
	}

	public void SetEastDoor(bool eastDoorStatus)
	{
		eastDoor = eastDoorStatus;
	}

}
