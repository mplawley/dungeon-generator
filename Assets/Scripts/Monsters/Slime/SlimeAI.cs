using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAI : MonoBehaviour
{
	[SerializeField]
	float slimeSpeed;

	// Use this for initialization
	void Start ()
	{

	}
	
	//Slimes are blind and move about randomly attacking. 
	void FixedUpdate ()
	{
		//Get current location
		Vector2 currentLocation = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);

		//Pick a random location within the specified bounds
		Vector2 movementTarget = new Vector2(
			Random.Range(MonsterManager.instance.xMin, MonsterManager.instance.xMax), 
			Random.Range(MonsterManager.instance.yMin, MonsterManager.instance.yMax));

		//Move to that location
		Vector2 movementVector = movementTarget - currentLocation;
		gameObject.transform.position = new Vector2(movementVector.normalized.x * slimeSpeed, movementVector.normalized.y * slimeSpeed);

		//Idle there a bit


		//Repeat

	}
}
