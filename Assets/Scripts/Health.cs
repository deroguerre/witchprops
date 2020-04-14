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

    private GameObject sceneManager = null;

    private void Start()
    {
        sceneManager = GameObject.Find("SceneManager");
    }

    public void TakeDamage(int amount)
    {
        if (!isServer)
            return;

        currentHealth -= amount;
        if (currentHealth <= 0)
        {

            currentHealth = maxHealth;
            NetworkLobbyCustom.nbSimplePlayer--;

            // endgame when all player are dead
            if (NetworkLobbyCustom.nbSimplePlayer <= 0)
            {
                sceneManager.GetComponent<DisplayEndGame>().RpcShowPanel(true);
            }

            // remove player
            // @todo make camera follow hunter
            NetworkServer.Destroy(gameObject);
        }
    }

    //[ClientRpc]
    //private void RpcRespawn()
    //{
    //    if (isLocalPlayer)
    //    {
    //        transform.position = Vector3.zero;
    //    }
    //}

    [Command]
    private void CmdDie()
    {
        //gameObject.GetComponent<Renderer>().material = deadMaterial;
        //NetworkServer.Destroy(gameObject);
    }
}