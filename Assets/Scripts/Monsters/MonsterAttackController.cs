using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttackController : MonoBehaviour 
{
	[SerializeField]
	int monsterDamage;

	[SerializeField]
	Transform monsterParentTransform;

	GameObject player;
	PlayerLife playerLife;
	Rigidbody2D playerRB;

	Vector2 monsterParentPosition;

	[SerializeField]
	float pushBackForce = 1.0f;

	// Use this for initialization
	void Start () 
	{
		player = GameObject.FindGameObjectWithTag("Player");
		playerRB = player.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		monsterParentPosition = new Vector2 (monsterParentTransform.position.x, monsterParentTransform.position.y);
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject == player)
		{
			PlayerLife.instance.ModifierPlayerLife(monsterDamage);

			//Push player away
			//Push to the right
			if (player.transform.position.x > monsterParentPosition.x)
			{
				playerRB.AddForce(player.transform.right * pushBackForce * 100);
			}
			//Push to the left
			else if (player.transform.position.x < monsterParentPosition.x)
			{
				playerRB.AddForce(player.transform.right * -pushBackForce * 100);
			}
			//Push up
			if (player.transform.position.y > monsterParentPosition.y)
			{
				playerRB.AddForce(player.transform.up * pushBackForce * 100);
			}
			//Push down
			else if (player.transform.position.y < monsterParentPosition.y)
			{
				playerRB.AddForce(player.transform.up * -pushBackForce * 100);
			}
		}
	}
}
