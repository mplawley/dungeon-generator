using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButtonController : MonoBehaviour
{
	[SerializeField]
	Text trainingPointsField;

	[SerializeField]
	GameObject distributeMorePrompt;

	[SerializeField]
	InputField nameField;

	int trainingPoints;

	public void StartGame()
	{
		//Get training points amount
		trainingPoints = int.Parse(trainingPointsField.GetComponent<Text>().text);

		//If the player hasn't finished distributing points....
		if (trainingPoints > 0)
		{
			distributeMorePrompt.SetActive(true);
		}
		else
		{
			MemoryManager.instance.allGameDataOnSingleton.dungeonData.playerName = nameField.text;
			MemoryManager.instance.Save();
			SceneController.instance.LoadScene("Game");
		}
	}
}
