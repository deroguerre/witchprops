using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System;

#pragma warning disable 618

[RequireComponent(typeof(Text))]
public class PartyTimer : NetworkBehaviour
{
    public Text timerText;
    public static bool partyIsRunning = true;
    public static bool hidePhase = true;
    [SyncVar] public float maxPartyTime;
    [SyncVar] public float maxHideTime;
    public GameObject playerSpawn;

    private GameObject hunterGo;

    // Start is called before the first frame update
    void Start()
    {
        timerText.text = maxPartyTime.ToString();
        Cursor.lockState = CursorLockMode.Locked;
        hunterGo = GameObject.FindWithTag("Hunter");
    }

    // Update is called once per frame
    void Update()
    {
        if (partyIsRunning)
        {
            Debug.Log("partyIsRunning");
            if (hidePhase)
            {
                Debug.Log("hidePhase");
                if (isServer)
                {
                    Debug.Log("is server");
                    maxHideTime -= Time.deltaTime;
                }

                int minutes = Mathf.FloorToInt(maxHideTime / 60F);
                int seconds = Mathf.FloorToInt(maxHideTime - minutes * 60);
                string formatedTime = string.Format("{0:0}:{1:00}", minutes, seconds);

                timerText.text = "Les props se cachent " + formatedTime;

                if (maxHideTime <= 0)
                {
                    //Debug.Log("tp hunter");
                    hidePhase = false;
                    CmdTpHunterToParty();
                }
            }
            else
            {
                Debug.Log("NOT hide phase");
                if (isServer)
                {
                    maxPartyTime -= Time.deltaTime;
                }

                if (maxPartyTime <= 0)
                {
                    maxPartyTime = 0;
                    gameObject.GetComponent<DisplayEndGame>().RpcShowPanel(false);
                    Destroy(this);
                }

                int minutes = Mathf.FloorToInt(maxPartyTime / 60F);
                int seconds = Mathf.FloorToInt(maxPartyTime - minutes * 60);
                string formatedTime = string.Format("{0:0}:{1:00}", minutes, seconds);

                timerText.text = "Chasse en cours : " + formatedTime;
            }
        }
        else
        {
            Debug.Log("party NOT Running");
        }
    }

    [Command]
    void CmdTpHunterToParty()
    {
        hunterGo.transform.position = playerSpawn.transform.position;
        hunterGo.transform.rotation = playerSpawn.transform.rotation;
    }
}