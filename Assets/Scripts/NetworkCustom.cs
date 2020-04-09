using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

#pragma warning disable 618

public class NetworkCustom : NetworkManager
{
	public GameObject spawnPoint;
	public GameObject hunterPrefab;
	public bool hunterIsActive = false;

	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
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
			player = NetworkStartPosition.Instantiate(playerPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
		}
		NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);

		Debug.Log("OnServerAddPlayer");
	}
}