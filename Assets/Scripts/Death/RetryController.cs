using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RetryController : MonoBehaviour
{
	[SerializeField]
	Button retryButton;

	public void OnRetry()
	{
		SceneController.instance.LoadScene("Start");
	}
}
