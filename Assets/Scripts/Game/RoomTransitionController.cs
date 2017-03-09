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

	public Vector3 newPlayerPosition = new Vector3();
	Vector3 newDungeonPosition;

	private float step = 1.0f;



	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.tag == "Door")
		{
			if (PlayerMotionController.instance.playerFacing == facing.up)
			{
				//Move dungeon....
				newDungeonPosition = dungeonHolder.position - transitionVectorUp;
				dungeonHolder.position = newDungeonPosition;

				//Put player at bottom of room....
				newPlayerPosition = new Vector3(gameObject.transform.position.x, PlayerMotionController.instance.boundary.yMin, 0f);
				gameObject.transform.position = newPlayerPosition;

			}
			else if (PlayerMotionController.instance.playerFacing == facing.right)
			{
				//Move dungeon....
				newDungeonPosition = dungeonHolder.position - transitionVectorRight;
				dungeonHolder.position = newDungeonPosition;

				//Put player at left of room....
				newPlayerPosition = new Vector3(PlayerMotionController.instance.boundary.xMin, gameObject.transform.position.y, 0f);
				gameObject.transform.position = newPlayerPosition;
			}
			else if (PlayerMotionController.instance.playerFacing == facing.down)
			{
				//Move dungeon....
				newDungeonPosition = dungeonHolder.position - transitionVectorDown;
				dungeonHolder.position = newDungeonPosition;

				//Put player at top of room....
				newPlayerPosition = new Vector3(gameObject.transform.position.x, PlayerMotionController.instance.boundary.yMax, 0f);
				gameObject.transform.position = newPlayerPosition;
			}
			else if (PlayerMotionController.instance.playerFacing == facing.left)
			{
				//Move dungeon....
				newDungeonPosition = dungeonHolder.position - transitionVectorLeft;
				dungeonHolder.position = newDungeonPosition;

				//Put player at right of room....
				newPlayerPosition = new Vector3(PlayerMotionController.instance.boundary.xMax, gameObject.transform.position.y, 0f);
				gameObject.transform.position = newPlayerPosition;
			}

		}
	}
}
