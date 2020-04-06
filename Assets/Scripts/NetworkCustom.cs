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

    private int _nbPlayer = 0;

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        GameObject player;

        // instantiate hunter if doesn't exist
        if (!hunterIsActive)
        {
            player = (GameObject) Instantiate(hunterPrefab);
            hunterIsActive = true;
        }
        else
        {
            player = Instantiate(playerPrefab) as GameObject;
        }
        player.transform.position = spawnPoint.transform.position;
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        _nbPlayer++;
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);
        _nbPlayer--;
    }
}