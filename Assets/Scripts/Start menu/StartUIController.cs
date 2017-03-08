/*
 * This script controls the save and load button functionality of the start screen. 
 * It also asks the user to select a save slot upon clicking the new game/load game buttons
 * buttons. The user's selection is passed on to the MemoryManager so that it knows
 * what persistentDataPath to use for storing character data.
 * 
 */

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;

public class StartUIController : MonoBehaviour
{
	[Header("UI elements")]
	public GameObject newGamePrompt;
	public GameObject loadGamePrompt;
	public Text textNoGameYetPrompt;

	public List<Toggle> toggleSaveList;
	public List<GameObject> toggleLoadList; //Get as GameObject so can set active/inactive
	public List<GameObject> toggleSaveOverwriteWarningList;

	[Header("Data")]
	public string characterToSaveLoad = "";
	bool slotExists = false;

	/* ================================================== UI FUNCTIONS ================================================== */

	//Click new game
	public void OnNewGame()
	{
		//Show new game panel and hide load game panel
		newGamePrompt.SetActive(true);
		loadGamePrompt.SetActive(false);

		//Search directory for data
		DirectorySearch();
	}

	//Click load game
	public void OnLoadGame()
	{
		//Hide new game panel and show load panel
		newGamePrompt.SetActive(false);
		loadGamePrompt.SetActive(true);

		//Search directory
		DirectorySearch();

		//In case the player has not yet created a character
		if (slotExists == false)
		{
			textNoGameYetPrompt.text = "You don't have a character saved yet. Please create one!";
		}
	}

	/* ================================================== DATA FUNCTIONS ================================================== */

	//Directory search for all character files, activating UI elements to let player know which character profiles can be loaded or saved without overwriting
	public void DirectorySearch()
	{
		//Check if directory exists. If not, create it....
		if (!Directory.Exists(Application.persistentDataPath + "/" + MemoryManager.instance.allGameDataOnSingleton.dungeonData.gameVersion))
		{
			Directory.CreateDirectory(Application.persistentDataPath + "/" + MemoryManager.instance.allGameDataOnSingleton.dungeonData.gameVersion);
		}

		DirectoryInfo dir = new DirectoryInfo(Application.persistentDataPath + "/" + MemoryManager.instance.allGameDataOnSingleton.dungeonData.gameVersion);
		FileInfo[] info = dir.GetFiles("*.*"); //Get all files in directory--that is, any that end with a dot, which is all of them

		//Prepare to display saved/loaded slots to the player
		int fileNameEndLength = 9; //To get last characters of a filename
		string slotName;

		foreach (FileInfo f in info)
		{
			//Get the part of the filename that corresponds to "SlotX" where X is file number
			slotName = f.ToString().Substring(Mathf.Max(0, f.ToString().Length - fileNameEndLength));

			//If a character slot exists in the persistentDataPath directory, display a corresponding slot to the player
			switch (slotName)
			{
			case ("Slot1.dat"):
				toggleSaveOverwriteWarningList[0].SetActive(true);
				toggleLoadList[0].SetActive(true);
				slotExists = true;
				break;
			case ("Slot2.dat"):
				toggleSaveOverwriteWarningList[1].SetActive(true);
				toggleLoadList[1].SetActive(true);
				slotExists = true;
				break;
			case ("Slot3.dat"):
				toggleSaveOverwriteWarningList[2].SetActive(true);
				toggleLoadList[2].SetActive(true);
				slotExists = true;
				break;
			default:
				break;
			}
		} 
	}

	//After player selects Create New Character and now selects a slot to save character data in
	public void OnSaveSlotSelection()
	{
		//The toggles this for-loop iterates through are part of a toggle group, so only one
		//will be selected at a time
		for (int toggleIdx = 0; toggleIdx < toggleSaveList.Count; toggleIdx++)
		{
			if (toggleSaveList[toggleIdx].isOn == true)
			{
				characterToSaveLoad = "/saveSlot" + (toggleIdx + 1).ToString(); //+1 to match SaveSlot numbering
			}
		}

		MemoryManager.instance.allGameDataOnSingleton.dungeonData.characterToSaveLoad = characterToSaveLoad;
		MemoryManager.instance.Save();
		SceneController.instance.LoadScene("Creation");
	}

	//After player selects Load Character and now selects a specific slot to load
	//NOTE: Load options are NOT error-checked here or in the Load() function. They are gated by UI load slots being defaulted to inactive and set active in 
	//the DirectorySearch() function above
	public void onLoadSlotSelection(string loadSlotSelection)
	{
		characterToSaveLoad = "/" + loadSlotSelection; //Adding forward-slash to the string to keep with filename requirements
		MemoryManager.instance.allGameDataOnSingleton.dungeonData.characterToSaveLoad = characterToSaveLoad; //Saving to MemoryManager singleton
		MemoryManager.instance.allGameDataOnSingleton.dungeonData.characterToSaveLoad = characterToSaveLoad; //Necessary to call before Character Sheet load or all data.values will be blank
		SceneController.instance.LoadScene("Game");
	}
}
