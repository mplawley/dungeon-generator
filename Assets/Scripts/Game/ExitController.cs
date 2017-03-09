/*
 * This script allows the player to exit the dungeon once she finds an exit door.
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitController : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.tag == "Player")
		{
			SceneController.instance.LoadScene("Win");
		}
	}
}
