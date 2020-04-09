using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkLobbyCustom : NetworkLobbyManager
{

    public GameObject spawnPoint;
	public GameObject newPlayerPrefab;
    public GameObject hunterPrefab;
	public bool hunterIsActive = false;

	private GameObject initialPlayer;

	//public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
	//{

	//	GameObject player;

	//	// instantiate hunter if doesn't exist
	//	if (!hunterIsActive)
	//	{
	//		player = (GameObject)NetworkStartPosition.Instantiate(hunterPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
	//		hunterIsActive = true;
	//	}
	//	else
	//	{
	//		player = NetworkStartPosition.Instantiate(playerPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
	//	}
	//	NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
	//}

	//public override void OnLobbyServerPlayersReady()
	//{
	//	base.OnLobbyServerPlayersReady();
	//	Debug.Log("Players ready !");
	//}

	//public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
	//{


	//	base.OnServerAddPlayer(conn, playerControllerId);
	//}

	//public override void OnLobbyClientConnect(NetworkConnection conn)
	//{
	//	Debug.Log("On Lobby Client Connect");
	//}
	//public override void OnServerSceneChanged(string sceneName)
	//{

	//	Debug.Log("On Server Scene Changed");
	//	base.OnServerSceneChanged(sceneName);
	//}


	public override GameObject OnLobbyServerCreateGamePlayer(NetworkConnection conn, short playerControllerId)
	{
		GameObject player;

		// instantiate hunter if doesn't exist
		if (!hunterIsActive)
		{
			player = (GameObject)NetworkStartPosition.Instantiate(hunterPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
			hunterIsActive = true;
		}
		else
		{
			player = NetworkStartPosition.Instantiate(newPlayerPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
		}
		NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);

		return player;
	}
}
