/*
 * This script is attached to a monster's parent object. 
 * The parent object's collider is its patrol zone.
 * The collider on its visuals is what blocks the player from occupying the same space as the monster.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour 
{
	//Fields
	[Header("Monster stats")]

	[SerializeField]
	float speed = 3f;

	[SerializeField]
	float attackRange = 1f;

	[Header("Script housekeeping")]

	[SerializeField]
	GameObject monsterVisuals;

	//Non-inspector fields
	GameObject player;
	Transform playerTransform;
	Vector2 monsterPosition2D;
	Vector2 monsterTarget;
	Vector2 randomPatrolPoint;
	Quaternion monsterRotation;
	bool playerInPatrolZone;

	void Start ()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		playerTransform = player.transform;
		playerInPatrolZone = false;

	}

	// Update is called once per frame
	void Update ()
	{
		if (playerInPatrolZone)
		{
			GetPlayerPosition();
		}
		else
		{
			Rest();
		}
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		//Player entered my patrol zone
		if (other.gameObject == player)
		{
			playerInPatrolZone = true;
		}
	}

	void OnTriggerExit2D (Collider2D other)
	{
		//Player exited my patrol zone
		if (other.gameObject == player) 
		{
			playerInPatrolZone = false;
		}
	}

	void Rest ()
	{
		//Do nothing
	}

	public void GetPlayerPosition()
	{
		if (Vector2.Distance (transform.position, playerTransform.position) > attackRange) 
		{
			monsterTarget = new Vector2 (playerTransform.position.x, playerTransform.position.y);

			//Move the monster towards the player
			MoveToTarget(monsterTarget);
		}
	}

	public void MoveToTarget(Vector2 monsterTarget)
	{
		//Get monster position
		monsterPosition2D = new Vector2 (transform.position.x, transform.position.y);

		//Move the monster towards the player
		transform.position = Vector2.MoveTowards(monsterPosition2D, monsterTarget, speed * Time.deltaTime);
	}
}
