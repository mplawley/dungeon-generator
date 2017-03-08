using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterLifeController : MonoBehaviour
{
	[SerializeField]
	GameObject parentMonsterObject;

	[SerializeField]
	float pushBackForce;

	Rigidbody2D rb;

	public int monsterLife;


	void Start()
	{
		rb = gameObject.GetComponent<Rigidbody2D> ();
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if(other.gameObject.tag == "Sword")
		{
			if(monsterLife <= 0)
			{
				Destroy (parentMonsterObject);
			}

			monsterLife -= 1;

			//Push monster back from the strike
			if (PlayerMotionController.instance.playerFacing == facing.left)
			{
				rb.AddForce(gameObject.transform.right * -pushBackForce * 100);
			}
			//Push to the left
			else if (PlayerMotionController.instance.playerFacing == facing.right)
			{
				rb.AddForce(gameObject.transform.right * pushBackForce * 100);
			}
			//Push up
			if (PlayerMotionController.instance.playerFacing == facing.up)
			{
				rb.AddForce(gameObject.transform.up * pushBackForce * 100);
			}
			//Push down
			else if (PlayerMotionController.instance.playerFacing == facing.down)
			{
				rb.AddForce(gameObject.transform.up * -pushBackForce * 100);
			}
		}
	}
}
