/*
 * This script allows the player to transition from room to room when she hits an exit
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTransitionController : MonoBehaviour
{
	public Transform dungeonHolder;
	public Vector3 transitionVectorLeft;
	public Vector3 transitionVectorRight;
	public Vector3 transitionVectorUp;
	public Vector3 transitionVectorDown;

	public Vector3 playerSouthPosition = new Vector3();
	public Vector3 playerWestPosition;
	public Vector3 playerEastPosition;
	public Vector3 playerNorthPosition;
	Vector3 newDungeonPosition;

	private float step;
	public float transitionSpeed;

	public bool touchedDoor = false;
	
	// Update is called once per frame
	void FixedUpdate()
	{
		if (touchedDoor)
		{
			if (PlayerMotionController.instance.playerFacing == facing.up)
			{
				//Move dungeon....
				newDungeonPosition = dungeonHolder.position - transitionVectorUp;
				step = transitionSpeed * Time.deltaTime;
				dungeonHolder.position = Vector3.MoveTowards(dungeonHolder.position, newDungeonPosition, step);

				//Put player at bottom of room....
				playerSouthPosition = new Vector3(PlayerMotionController.instance.boundary.yMin, gameObject.transform.position.y, 0f);
				gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, playerSouthPosition, step);

			}
			else if (PlayerMotionController.instance.playerFacing == facing.right)
			{
				//Move dungeon....
				newDungeonPosition = dungeonHolder.position - transitionVectorRight;
				step = transitionSpeed * Time.deltaTime;
				dungeonHolder.position = Vector3.MoveTowards(dungeonHolder.position, newDungeonPosition, step);

				//Put player at left of room....
				playerWestPosition = new Vector3(PlayerMotionController.instance.boundary.xMin, gameObject.transform.position.y, 0f);
				gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, playerWestPosition, step);
			}
			else if (PlayerMotionController.instance.playerFacing == facing.down)
			{
				//Move dungeon....
				newDungeonPosition = dungeonHolder.position - transitionVectorDown;
				step = transitionSpeed * Time.deltaTime;
				dungeonHolder.position = Vector3.MoveTowards(dungeonHolder.position, newDungeonPosition, step);

				//Put player at top of room....
				playerWestPosition = new Vector3(gameObject.transform.position.x, PlayerMotionController.instance.boundary.yMax, 0f);
				gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, playerWestPosition, step);
			}
			else if (PlayerMotionController.instance.playerFacing == facing.left)
			{
				//Move dungeon....
				newDungeonPosition = dungeonHolder.position - transitionVectorLeft;
				step = transitionSpeed * Time.deltaTime;
				dungeonHolder.position = Vector3.MoveTowards(dungeonHolder.position, newDungeonPosition, step);

				//Put player at right of room....
				playerWestPosition = new Vector3(PlayerMotionController.instance.boundary.xMax, gameObject.transform.position.y, 0f);
				gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, playerWestPosition, step);
			}
			Invoke("GrantPlayerMovement", 5.0f);
		}
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.tag == "Door")
		{
			touchedDoor = true;
		}
	}

	public void GrantPlayerMovement()
	{
		gameObject.GetComponent<PlayerMotionController>().enabled = true;
		touchedDoor = false;
	}
}
