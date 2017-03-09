/*
 * This script controls the game's sounds!
 * 
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
	public static SoundManager instance;
	public List<AudioSource> audios = new List<AudioSource>();

	[SerializeField]
	Camera mainCamera;

	// Creating a singleton

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

		//Get main camera
		mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
	}

	//Play a sound by fileName
	public void PlaySound(string audioClipName)
	{
		for (int i = 0; i < audios.Count; i++)
		{
			if (audioClipName == audios[i].name)
			{
				audios[i].Play();
			}
		}        

	}
}
