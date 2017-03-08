/*
 * This script handles the game's memory and data management.
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class MemoryManager : MonoBehaviour
{
	[Header("Data settings")]
	public static MemoryManager instance;
	public AllGameData allGameDataOnSingleton = new AllGameData();

	// Singleton
	void Awake ()
	{
		if (instance == null)
		{
			DontDestroyOnLoad(gameObject);
			instance = this;
		}
		else if (instance != this)
		{
			Destroy (gameObject);
		}
	}

/*
 * *************************************************************************************************
 * *************************************************************************************************
 * *************************************** FUNDAMENTAL MEMORY MANAGEMENT ***************************
 * ********************************************* FILE I/O ******************************************
 * *************************************************************************************************
 */

	public void Save()
	{
		//Check if directory exists. If not, create it....
		if (!Directory.Exists(Application.persistentDataPath + "/" + MemoryManager.instance.allGameDataOnSingleton.dungeonData.gameVersion))
		{
			Directory.CreateDirectory(Application.persistentDataPath + "/" + MemoryManager.instance.allGameDataOnSingleton.dungeonData.gameVersion);
		}

		//Use a binary formatter to write
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "/" + MemoryManager.instance.allGameDataOnSingleton.dungeonData.gameVersion + MemoryManager.instance.allGameDataOnSingleton.dungeonData.characterToSaveLoad + ".dat");

		//Write data from singleton to memory
		AllGameData allGameDataToMemory = allGameDataOnSingleton;

		//Serialize data
		bf.Serialize(file, allGameDataToMemory);

		//File I/O
		file.Close();
	}

	public void Load()
	{
		if (File.Exists(Application.persistentDataPath + "/" + MemoryManager.instance.allGameDataOnSingleton.dungeonData.gameVersion + MemoryManager.instance.allGameDataOnSingleton.dungeonData.characterToSaveLoad + ".dat"))
		{
			//Use binary formatter to read
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/" + MemoryManager.instance.allGameDataOnSingleton.dungeonData.gameVersion + MemoryManager.instance.allGameDataOnSingleton.dungeonData.characterToSaveLoad + ".dat", FileMode.Open);

			//Deserialize data from memory
			AllGameData allGameDataFromMemory = (AllGameData)bf.Deserialize(file);

			//File I/O
			file.Close();

			//Write data from memory to singleton
			allGameDataOnSingleton = allGameDataFromMemory;
		}
		else
		{
			Debug.LogError("File does not exist.");
		}
	}

	//Clear ONLY currently loaded singleton data. Used when exiting a chapter.
	public void ClearOnlySingletonData()
	{
		allGameDataOnSingleton.dungeonData.ClearData();
		//TODO: If you ever add to this game, also clear, for instance, Chapter2Data, MarketData, etc.
	}
}

/*
 * *************************************************************************************************
 * *************************************************************************************************
 * *************************************** DATA ****************************************************
 * *************************************************************************************************
 * *************************************************************************************************
 */

[Serializable]
public class DungeonGeneratorData
{
	//Version
	public string characterToSaveLoad = "";
	public string gameVersion = "1.00";

	//Player
	public string playerName = "";
	public int playerLife;
	public int playerSpeed;
	public int playerStrength;
	public int playerWisdom;

	//Later chapter completed. Defaults to 0 for none. 1 for chapter 1 completed, 10 for chapter 10 completed, etc.
	public bool firstPlayThrough = false;

	//Class methods
	public void ClearData()
	{
		playerName = "";
		playerLife = 0;
		playerSpeed = 0;
		playerStrength = 0;
		playerWisdom = 0;
	}
}

[Serializable]
public class AllGameData //I chose to have an AllGameData in hypothetical anticipation of needing, for instance, MarketData, Chapter2Data, etc.
{
	//Game 1
	public DungeonGeneratorData dungeonData = new DungeonGeneratorData();

	//Chapter 2
	//TODO: For future building!

	//Market
	//TODO: If this app ever needed, for instance, in-app-purchases to be implemented....
}
