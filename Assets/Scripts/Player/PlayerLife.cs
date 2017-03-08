/*
 * This script handles the player's life and the player's death condition
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
	public static PlayerLife instance;

	[SerializeField]
	int maxLife;

	[SerializeField]
	List<GameObject> playerHearts = new List<GameObject>();

	Animator animator;

	void Awake()
	{
		//Singleton
		if (instance == null)
		{
			DontDestroyOnLoad(gameObject);
			instance = this;
		}
		else if (instance != this)
		{
			Destroy(gameObject);
		}
	}

	void Start()
	{
		animator = gameObject.GetComponent<Animator>();
	}

	public void ModifierPlayerLife(int howMuch)
	{
		int life = MemoryManager.instance.allGameDataOnSingleton.dungeonData.playerLife;

		life += howMuch;

		//Cap life
		if (life > maxLife)
		{
			life = maxLife;
		}

		//Set hearts display..
		SetPlayerHearts();

		//Death
		if (life < 0)
		{
			animator.SetTrigger ("death");
			Invoke ("PlayerDead", 1.0f);
		}
	}

	//Display player life...
	public void SetPlayerHearts()
	{
		int life = MemoryManager.instance.allGameDataOnSingleton.dungeonData.playerLife;

		//Reset hearts
		for (int i = 0; i < playerHearts.Count; i++)
		{
			playerHearts [i].SetActive (false);
		}

		//Set hearts anew
		for (int i = 0; i < life; i++)
		{
			playerHearts[i].SetActive (true);
		}
	}

	//Player death
	void PlayerDead()
	{
		SceneController.instance.LoadScene("Death");
	}
}
