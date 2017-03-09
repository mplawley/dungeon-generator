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
			//Death?
			if(monsterLife <= 0)
			{
				Destroy (parentMonsterObject);
			}

			//Damage
			int playerStrength = MemoryManager.instance.allGameDataOnSingleton.dungeonData.playerStrength;
			monsterLife -= playerStrength;

			//Push monster back from the strike
			if (PlayerMotionController.instance.playerFacing == facing.left)
			{
				rb.AddForce(gameObject.transform.right * -pushBackForce * 100 * playerStrength);
			}
			//Push to the left
			else if (PlayerMotionController.instance.playerFacing == facing.right)
			{
				rb.AddForce(gameObject.transform.right * pushBackForce * 100 * playerStrength);
			}
			//Push up
			if (PlayerMotionController.instance.playerFacing == facing.up)
			{
				rb.AddForce(gameObject.transform.up * pushBackForce * 100 * playerStrength);
			}
			//Push down
			else if (PlayerMotionController.instance.playerFacing == facing.down)
			{
				rb.AddForce(gameObject.transform.up * -pushBackForce * 100 * playerStrength);
			}
		}
	}
}
