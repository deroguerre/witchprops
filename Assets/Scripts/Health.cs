using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
#pragma warning disable 618

public class Health : NetworkBehaviour
{
    public const int maxHealth = 100;
    public Material deadMaterial;
    [SyncVar] public int currentHealth = maxHealth;

    public void TakeDamage(int amount)
    {
        if (!isServer)
            return;

        Debug.Log("nb player" + NetworkLobbyCustom.nbSimplePlayer);

        currentHealth -= amount;
        if (currentHealth <= 0)
        {

            currentHealth = maxHealth;
            NetworkLobbyCustom.nbSimplePlayer--;

            // endgame when all player are dead
            if (NetworkLobbyCustom.nbSimplePlayer <= 0)
            {
                Debug.Log("nb player" + NetworkLobbyCustom.nbSimplePlayer);
                CmdEndGame();
            }

            // remove player
            // @todo make camera follow hunter
            NetworkServer.Destroy(gameObject);
        }
    }

    [ClientRpc]
    private void RpcRespawn()
    {
        if (isLocalPlayer)
        {
            transform.position = Vector3.zero;
        }
    }

    [Command]
    private void CmdDie()
    {
        //gameObject.GetComponent<Renderer>().material = deadMaterial;
        //NetworkServer.Destroy(gameObject);
    }

    [Command]
    private void CmdEndGame()
    {
        Debug.Log("End game");
        NetworkLobbyCustom.hunterIsActive = false;
        NetworkLobbyCustom.nbSimplePlayer = 0;
        NetworkManager.singleton.ServerChangeScene("Lobby");
    }
}