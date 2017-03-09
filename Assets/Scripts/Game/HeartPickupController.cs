using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartPickupController : MonoBehaviour
{

	//Trap hurts the player
	public int amountHealed;

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.tag == "Player")
		{
			PlayerLife.instance.ModifierPlayerLife(amountHealed);
			Destroy(gameObject);
		}
	}
}
