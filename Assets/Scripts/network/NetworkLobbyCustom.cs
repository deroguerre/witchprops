using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

#pragma warning disable 618

public class NetworkLobbyCustom : NetworkLobbyManager
{
    public GameObject newPlayerPrefab;
    public GameObject hunterPrefab;
    public GameObject playerSpawn;
    public GameObject hunterSpawn;
    public static bool HunterIsActive = false;
    public static int NbSimplePlayer = 0;

    private int _spawnSpacing = 1;

    public override void OnLobbyStartHost()
    {
        base.OnLobbyStartHost();
        resetScene();
    }

    public override GameObject OnLobbyServerCreateGamePlayer(NetworkConnection conn, short playerControllerId)
    {
        GameObject player;

        // instantiate hunter if doesn't exist
        if (!HunterIsActive)
        {
            player = Instantiate(hunterPrefab, hunterSpawn.transform.position,
                hunterSpawn.transform.rotation);
            HunterIsActive = true;
        }
        else
        {
            Vector3 initialSpawnPosition = playerSpawn.transform.position;
            Vector3 spawnPosition = new Vector3(initialSpawnPosition.x + _spawnSpacing, initialSpawnPosition.y,
                initialSpawnPosition.z);
            _spawnSpacing += _spawnSpacing;

            player = Instantiate(newPlayerPrefab, spawnPosition, playerSpawn.transform.rotation);
        }

        return player;
    }
    
    public override void OnLobbyServerPlayersReady()
    {
        base.OnLobbyServerPlayersReady();
        NbSimplePlayer = numPlayers - 1;
        PartyTimer.partyIsRunning = true;
    }

    public override void OnStopClient()
    {
        base.OnStopClient();
        resetScene();
        Debug.Log("OnStopClient");
    }

    public override void OnStopServer()
    {
        base.OnStopServer();
        resetScene();
        Debug.Log("OnStopServer");
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);
        resetScene();
        Debug.Log("disconnected !!!");
    }


    public void resetScene()
    {
        Cursor.lockState = CursorLockMode.None;
        HunterIsActive = false;
        NbSimplePlayer = 0;
        PartyTimer.partyIsRunning = true;
        PartyTimer.hidePhase = true;

        Debug.Log("resetScene");
    }

    // public override void OnServerDisconnect(NetworkConnection conn)
    // {
    //     base.OnServerDisconnect(conn);
    //     Cursor.lockState = CursorLockMode.None;
    //     Debug.Log("OnServerDisconnect");
    // }

    // public override void OnClientDisconnect(NetworkConnection conn)
    // {
    //     base.OnClientDisconnect(conn);
    //     Cursor.lockState = CursorLockMode.None;
    //     Debug.Log("OnClientDisconnect");
    // }

    // public override void OnStopHost()
    // {
    //     base.OnStopHost();
    //     Cursor.lockState = CursorLockMode.None;
    //     Debug.Log("OnStopHost");
    // }
}