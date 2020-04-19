using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

#pragma warning disable 618

public class Health : NetworkBehaviour
{
    public int maxHealth = 100;
    [SyncVar] private int _currentHealth;

    private GameObject _sceneManager;

    private void Start()
    {
        _currentHealth = maxHealth;
        _sceneManager = GameObject.Find("SceneManager");
    }

    public void TakeDamage(int amount)
    {
        if (!isServer)
            return;

        _currentHealth -= amount;
        RpcDamageEffect();

        if (_currentHealth <= 0)
        {
            _currentHealth = maxHealth;
            CustomNetworkLobby.NbSimplePlayer--;

            // endgame when all player are dead
            if (CustomNetworkLobby.NbSimplePlayer <= 0)
            {
                _sceneManager.GetComponent<DisplayEndGame>().RpcShowPanel(true);
            }

            // remove player
            // @todo make camera follow hunter
            NetworkServer.Destroy(gameObject);
        }
    }

    [ClientRpc]
    private void RpcDamageEffect()
    {
        // GetComponent<MeshRenderer>().material.color = Color.blue;
        StartCoroutine(DamageEffectCoroutine());
    }

    IEnumerator DamageEffectCoroutine()
    {
        //Color tempDefaultColor = GetComponent<MeshRenderer>().material.color;
        GetComponent<MeshRenderer>().material.color = Color.red;
        yield return new WaitForSeconds(0.05f);
        GetComponent<MeshRenderer>().material.color = Color.white;
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