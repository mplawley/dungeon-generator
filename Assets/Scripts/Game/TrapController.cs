using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapController : MonoBehaviour
{
	//Trap hurts the player
	public int damageDealt = -1;

	void OnTriggerEnter2D (Collider2D coll)
	{
		if (coll.gameObject.tag == "Player")
		{
			PlayerLife.instance.ModifierPlayerLife(damageDealt);
		}
	}
}
