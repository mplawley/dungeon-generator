using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum facing {left, up, right, down};

public class PlayerMotionController : MonoBehaviour
{
	public static PlayerMotionController instance;

	//public fields
	public facing playerFacing;

	//Inspector fields
	[SerializeField]
	float speed;

	//non-inspector fields
	public Boundary boundary;
	public float moveHorizontal;
	public float moveVertical;
	Vector3 movement;
	Rigidbody2D rb;
	Animator animator;

	void Awake()
	{
		if (instance != this)
		{
			instance = this;
		}
		else
		{
			Destroy(this);
		}
	}

	// Use this for initialization
	void Start ()
	{
		//Component hookups
		rb = gameObject.GetComponent<Rigidbody2D>();
		animator = gameObject.GetComponent<Animator>();

		//Set player speed
		MemoryManager.instance.Load();
		speed = MemoryManager.instance.allGameDataOnSingleton.dungeonData.playerSpeed;
	}

	//Actuate the player based on input
	void FixedUpdate()
	{
		//Movement input
		moveHorizontal = Input.GetAxisRaw ("Horizontal");
		moveVertical = Input.GetAxisRaw ("Vertical");

		//Movement calculation and execution
		movement = new Vector3 (moveHorizontal, moveVertical, 0.0f);
		rb.velocity = movement * speed;

		//Movement bounds
		rb.position = new Vector3 
			(
				Mathf.Clamp (rb.position.x, boundary.xMin, boundary.xMax), 
				Mathf.Clamp (rb.position.y, boundary.yMin, boundary.yMax),
				0.0f
			);

		//Set animations
		if (moveHorizontal < 0)
		{
			animator.SetTrigger("moveLeft");
			playerFacing = facing.left;
		}
		else if (moveHorizontal > 0)
		{
			animator.SetTrigger("moveRight");
			playerFacing = facing.right;
		}
		else if (moveVertical < 0)
		{
			animator.SetTrigger("moveDown");
			playerFacing = facing.down;
		}
		else if (moveVertical > 0)
		{
			animator.SetTrigger("moveUp");
			playerFacing = facing.up;
		}
	}
}

//Class for movement boundaries on the screen
[System.Serializable]
public class Boundary
{
	public float xMin, xMax, yMin, yMax;
}
