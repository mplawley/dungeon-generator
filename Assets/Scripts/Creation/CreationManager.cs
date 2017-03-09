/*
 * This script controls character creation....
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreationManager : MonoBehaviour
{
	[Header("UI references")]
	public Text trainingPointsField;
	public Text speedField;
	public Text strengthField;
	public Text wisdomField;
	public Text statExplanation;

	public GameObject outOfPointsPrompt;

	//Fields
	int trainingPointsAmount;
	int speedAmount = 1;
	int strengthAmount = 1;
	int wisdomAmount = 1;

	[SerializeField]
	GameObject distributeMorePrompt;

	// Use this for initialization
	void Start()
	{
		trainingPointsAmount = int.Parse(trainingPointsField.GetComponent<Text>().text);
	}

	public void RaiseStat(string statName)
	{
		//Update values
		if (trainingPointsAmount > 0)
		{
			switch (statName)
			{
			case "speed":
				speedAmount += 1;
				trainingPointsAmount -= 1;
				statExplanation.text = "Speed determines how fast you move. Beware: low values will make you very slow!";
				break;
			case "strength":
				strengthAmount += 1;
				trainingPointsAmount -= 1;
				statExplanation.text = "Strength determines how much damage you deal and how far you push enemies. If you hit hard enough, some enemies will flee from you!";
				break;
			case "wisdom":
				wisdomAmount += 1;
				trainingPointsAmount -= 1;
				statExplanation.text = "Wisdom determines how strong your magic is.";
				break;
			default:
				break;
			}
		}
		else
		{
			outOfPointsPrompt.SetActive(true);
		}

		//Update UI
		UpdateUI();

		//Update memory singleton
		UpdateMemory();
	}

	public void LowerStat(string statName)
	{
		//Update values
		if (statName == "speed")
		{
			if (speedAmount > 1)
			{
				speedAmount -= 1;
				trainingPointsAmount += 1;
			}
			else if (speedAmount <= 1)
			{
				return;
			}
		}

		else if (statName == "strength")
		{
			if (strengthAmount > 1)
			{
				strengthAmount -= 1;
				trainingPointsAmount += 1;
			}
			else if (strengthAmount <= 1)
			{
				return;
			}
		}

		if (statName == "wisdom")
		{
			if (wisdomAmount > 1)
			{
				wisdomAmount -= 1;
				trainingPointsAmount += 1;
			}
			else if (wisdomAmount <= 1)
			{
				return;
			}
		}

		//Clear out-of-points field....
		if (trainingPointsAmount > 0)
		{
			outOfPointsPrompt.SetActive(false);
		}


		//Update UI
		UpdateUI();

		//Update memory singleton
		UpdateMemory();
	}

	void UpdateUI()
	{
		//Update UI
		trainingPointsField.GetComponent<Text>().text = trainingPointsAmount.ToString();
		speedField.GetComponent<Text>().text = speedAmount.ToString();
		strengthField.GetComponent<Text>().text = strengthAmount.ToString();
		wisdomField.GetComponent<Text>().text = wisdomAmount.ToString();

		//Clear warning prompt...
		distributeMorePrompt.SetActive(false);
	}

	void UpdateMemory()
	{
		MemoryManager.instance.allGameDataOnSingleton.dungeonData.playerLife = 5;
		MemoryManager.instance.allGameDataOnSingleton.dungeonData.playerSpeed = speedAmount;
		MemoryManager.instance.allGameDataOnSingleton.dungeonData.playerStrength = strengthAmount;
		MemoryManager.instance.allGameDataOnSingleton.dungeonData.playerWisdom = wisdomAmount;
	}
}
