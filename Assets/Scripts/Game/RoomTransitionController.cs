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

	bool touchedDoor = false;
	
	// Update is called once per frame
	void Update()
	{
		if (touchedDoor)
		{
			if (PlayerMotionController.instance.playerFacing == facing.up)
			{
				print("Facing up");
				dungeonHolder.transform.position += (gameObject.transform.position - transitionVectorUp) * 10 * Time.deltaTime;
			}
			else if (PlayerMotionController.instance.playerFacing == facing.right)
			{
				print("Facing right");
				dungeonHolder.transform.position += (gameObject.transform.position - transitionVectorRight) * 10 * Time.deltaTime;
			}
			else if (PlayerMotionController.instance.playerFacing == facing.down)
			{
				print("Facing down");
				dungeonHolder.transform.position += (gameObject.transform.position - transitionVectorDown) * 10 * Time.deltaTime;
			}
			else if (PlayerMotionController.instance.playerFacing == facing.left)
			{
				print("Facing left");
				dungeonHolder.transform.position += (gameObject.transform.position - transitionVectorLeft) * 10 * Time.deltaTime;
			}
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
	}
}
