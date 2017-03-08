using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
	public static PlayerLife instance;
	public int life;

	[SerializeField]
	int maxLife;

	Animator animator;

	void Awake()
	{
		if (instance != this)
		{
			instance = this;
		}
		else
		{
			Destroy (this);
		}
	}

	void Start()
	{
		animator = gameObject.GetComponent<Animator>();
	}

	public void ModifierPlayerLife(int howMuch)
	{
		life += howMuch;

		//Cap life
		if (life > maxLife)
		{
			life = maxLife;
		}

		//Death
		if (life < 0)
		{
			animator.SetTrigger ("death");
			Invoke ("PlayerDead", 1.0f);
		}
	}

	//Display player life...

	//Player death
	void PlayerDead()
	{
		SceneController.instance.LoadScene("Death");
	}
}
