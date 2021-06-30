using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System;

public class UDP : MonoBehaviour
{

    static UdpClient udp;
    IPEndPoint remoteEP = null;
    public float[,] ball_infoPosition = new float[3, 2];
    public bool[] ball_infoFlag = { false, false, false };
    public float[,] target_infoPosition = new float[3, 8];
    public bool[] target_infoFlag = { false, false, false };
    public int ballcount;
    public int targetcount;
    // Use this for initialization
    void Start()
    {
        int LOCA_LPORT = 50007;

        udp = new UdpClient(LOCA_LPORT);
        udp.Client.ReceiveTimeout = 200;
    }

    // Update is called once per frame
    void Update()
    {
        int i = 0;
        IPEndPoint remoteEP = null;
        byte[] data = udp.Receive(ref remoteEP);
        string text = Encoding.UTF8.GetString(data);
        //多分Replaceの処理は分けないと死
        string[] arr = text.Split('\n');
        i++;
        Debug.Log(text);
        //的の処理
        targetcount = int.Parse(arr[i]);
        for (int j = 0; j < targetcount; j++)
        {
            string[] arr_posi = arr[i].Split(' ');
            int target_infoID = int.Parse(arr_posi[0]);
            target_infoFlag[target_infoID] = true;
            for (int k = 1; k < 9; k++)
            {
                target_infoPosition[target_infoID, k - 1] = float.Parse(arr_posi[k]);
            }
            i++;
        }

        //ボールの処理
        ballcount = int.Parse(arr[i]);
        i++;
        for (int j = 0; j < ballcount; j++)
        {
            string[] arr_posi = arr[i].Split(' ');
            int ball_infoID = int.Parse(arr_posi[0]);
            ball_infoFlag[ball_infoID] = true;
            for (int k = 1; k < 2; k++)
            {
                ball_infoPosition[ball_infoID, k - 1] = float.Parse(arr_posi[k]);
            }
            i++;
        }
    }
}