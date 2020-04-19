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

		endGamePanel.gameObject.SetActive(true);
		endGameText.gameObject.SetActive(true);
		endGameButton.gameObject.SetActive(true);

		// stop timer
		PartyTimer.partyIsRunning = false;

		Cursor.lockState = CursorLockMode.None;
	}

	public void backToMainMenu()
	{
		Debug.Log("BackToMainMenu");
		PartyTimer.hidePhase = true;
		PartyTimer.partyIsRunning = true;

		CustomNetworkLobby.singleton.StopClient();

		if (isServer)
		{
			CustomNetworkLobby.singleton.StopServer();
		}

		//NetworkManager.singleton.client.Disconnect();
	}
}
