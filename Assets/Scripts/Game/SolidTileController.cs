using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolidTileController : MonoBehaviour
{
	GameObject player;
	Vector3 tilePosition;
	Rigidbody2D playerRB;

	[SerializeField]
	float pushBackForce = 1.0f;

	// Use this for initialization
	void Start()
	{
		player = GameObject.FindGameObjectWithTag ("Player");
		playerRB = player.GetComponent<Rigidbody2D>();
		tilePosition = gameObject.transform.position;
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject == player)
		{
			//Push player away
			//Push to the right
			if (player.transform.position.x > tilePosition.x)
			{
				playerRB.AddForce(player.transform.right * pushBackForce * 100);
			}
			//Push to the left
			else if (player.transform.position.x < tilePosition.x)
			{
				playerRB.AddForce(player.transform.right * -pushBackForce * 100);
			}
			//Push up
			if (player.transform.position.y > tilePosition.y)
			{
				playerRB.AddForce(player.transform.up * pushBackForce * 100);
			}
			//Push down
			else if (player.transform.position.y < tilePosition.y)
			{
				playerRB.AddForce(player.transform.up * -pushBackForce * 100);
			}
		}
	}
}
