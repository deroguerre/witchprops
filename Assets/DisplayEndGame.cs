using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
#pragma warning disable 618

public class DisplayEndGame : NetworkBehaviour
{
	public GameObject endGamePanel;
	public Text endGameText;
	public Button endGameButton;

	[ClientRpc]
	public void RpcShowPanel(bool hunterWin)
	{

		if (hunterWin)
		{
			endGameText.text = "Le chasseur remporte la victoire !";
		}
		else
		{
			endGameText.text = "Les props remportent la victoire !";
		}

		Debug.Log("Show Win message");

		endGamePanel.gameObject.SetActive(true);
		endGameText.gameObject.SetActive(true);
		endGameButton.gameObject.SetActive(true);
	}

	public void backToMainMenu()
	{
		Debug.Log("End game");
		NetworkLobbyCustom.hunterIsActive = false;
		NetworkLobbyCustom.nbSimplePlayer = 0;

		if (isServer)
		{
			NetworkManager.singleton.StopServer();
		}

		NetworkManager.singleton.StopClient();
		NetworkManager.singleton.client.Disconnect();
	}
}
