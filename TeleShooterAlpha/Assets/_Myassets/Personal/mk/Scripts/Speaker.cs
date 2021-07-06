using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Speaker : MonoBehaviour
{
    //先ほど作成したクラス
    public SerialHandler serialHandler;
    GameObject TargetManeger;
    TargetManager script;

    void Start()
    {
        TargetManeger = GameObject.Find("TargetManager");
        script = TargetManeger.GetComponent<TargetManager>();
        serialHandler.OnDataReceived += OnDataReceived;
    }

    void Update()
    {
        //フラグ受け取り
        bool[] flag_bool = script.destroyFlag;
        double f = 0;
        for(int i = 0; i < 3; i++)
        {
            if (flag_bool[i])
            {
                f+= Math.Pow(2, i);
            }
        }
        int ff = (int)f;
        string Flag = ff.ToString();
        //文字列を送信
        serialHandler.Write(Flag);
    }
    void OnDataReceived(string message)
    {
        var data = message.Split(
                new string[] { "\t" }, System.StringSplitOptions.None);
        Debug.Log("return"+data[0]);
        if (data.Length < 2) return;

        try
        {
        }
        catch (System.Exception e)
        {
            Debug.LogWarning(e.Message);
        }
    }

}