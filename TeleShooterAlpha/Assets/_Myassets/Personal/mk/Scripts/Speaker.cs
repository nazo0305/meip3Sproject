using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    }

    void Update()
    {
        //フラグ受け取り
        bool[] flag_bool = script.destroyFlag;
        string Flag = "";
        for (int i = 0; i < 3; i++)
        {
            if (flag_bool[i])
            {
                Flag += "1";
            }
            else
            {
                Flag += "0";
            }
        }
        Debug.Log(Flag);
        //文字列を送信
        serialHandler.Write(Flag);
    }


}