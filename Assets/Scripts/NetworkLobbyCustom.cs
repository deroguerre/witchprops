using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
#pragma warning disable 618

public class NetworkLobbyCustom : NetworkLobbyManager
{

	public GameObject spawnPoint;
	public GameObject newPlayerPrefab;
	public GameObject hunterPrefab;
	public static bool hunterIsActive = false;
	public static int nbSimplePlayer = 0;

	private int spacing = 1;

	public override void OnLobbyServerPlayersReady()
	{
		base.OnLobbyServerPlayersReady();
		nbSimplePlayer = numPlayers - 1;
	}

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
			Vector3 initialSpawnPosition = spawnPoint.transform.position;
			Vector3 spawnPosition = new Vector3(initialSpawnPosition.x + spacing, initialSpawnPosition.y, initialSpawnPosition.z);
			spacing += spacing;

			player = NetworkStartPosition.Instantiate(newPlayerPrefab, spawnPosition, spawnPoint.transform.rotation);
		}
		//NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);

		return player;
	}
}
