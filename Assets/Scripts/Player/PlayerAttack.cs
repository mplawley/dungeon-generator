/*
 * This script controls the player's attack animations and functionality 
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
	public Transform attackPosition;
	public float attackRadius;
	public int maxObjectsHit = 5;
	public Collider2D[] objectsHit;
	public LayerMask selectObjectsToHit;

	[SerializeField]
	GameObject swordLeft, swordUp, swordRight, swordDown;

	[SerializeField]
	float attackRetractTime;

	SpriteRenderer playerSpriteRenderer;

	Animator animator;

	void Start()
	{
		objectsHit = new Collider2D[maxObjectsHit];
		animator = gameObject.GetComponent<Animator>();
		playerSpriteRenderer = gameObject.GetComponent<SpriteRenderer> ();
	}

	void Update()
	{
		if(Input.GetMouseButtonDown (0) || Input.GetKeyDown("space"))
		{
			//Get facing and turn on sword state...
			if(PlayerMotionController.instance.playerFacing == facing.left)
			{
				playerSpriteRenderer.enabled = false;
				swordLeft.SetActive (true);
				Invoke ("SwordOff", attackRetractTime);
			}
			else if(PlayerMotionController.instance.playerFacing == facing.up)
			{
				playerSpriteRenderer.enabled = false;
				swordUp.SetActive (true);
				Invoke ("SwordOff", attackRetractTime);
			}
			else if(PlayerMotionController.instance.playerFacing == facing.right)
			{
				playerSpriteRenderer.enabled = false;
				swordRight.SetActive (true);
				Invoke ("SwordOff", attackRetractTime);
			}
			else if(PlayerMotionController.instance.playerFacing == facing.down)
			{
				playerSpriteRenderer.enabled = false;
				swordDown.SetActive (true);
				Invoke ("SwordOff", attackRetractTime);
			}
		}
	}

	void SwordOff()
	{
		//Turn off sword state
		swordLeft.SetActive(false);
		swordUp.SetActive(false);
		swordRight.SetActive(false);
		swordDown.SetActive(false);

		//Turn sprite renderer back on
		playerSpriteRenderer.enabled = true;
	}
}
