﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System;

public class PartyTimer : NetworkBehaviour
{
    public Text timerText;
    [SyncVar] private float timer = 60;

    // Start is called before the first frame update
    void Start()
    {
        timerText.text = timer.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if(isServer)
        {
            timer -= Time.deltaTime;
            //Debug.Log(timer);
        }

        if (timer <= 0)
        {
            timer = 0;
            this.gameObject.GetComponent<DisplayEndGame>().RpcShowPanel(false);
            Destroy(this);
        }

        int minutes = Mathf.FloorToInt(timer / 60F);
        int seconds = Mathf.FloorToInt(timer - minutes * 60);
        string niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);

        //float truncateTime = Mathf.Round(timer * 10.0f) / 10.0f;
        //Debug.Log(truncateTime);
        //var ts = TimeSpan.FromSeconds(truncateTime);
        timerText.text = niceTime;
    }
}
