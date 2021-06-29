using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class TargetManager : MonoBehaviourPunCallbacks
{
    public int targetId;
    public Vector2[] targetPosition=new Vector2[3];//とりあえず3個
    public int count;//認識してる的の数
    public int target_now;
    public GameObject[] targetArray= new GameObject[3];//とりあえず3個

    [SerializeField]GameObject canvas;

    // Start is called before the first frame update
    void Start()
    {
        count = 0;     //認識されている四角形
        target_now = 0; //的オブジェクトの数(爆発エフェクト等含む)

    }

    // Update is called once per frame
    void Update()
    {
        //必要Target数を取得
        count = 1; //後で消す
        //Target数を取得
        target_now =0;
        foreach(GameObject target in targetArray)
        {
            if(target!=null)
            {
                target_now++;
            }
        }
        //目標の数が足りていなければTargetを生成
        if ( target_now < count)
        {
            //的をそれぞれ識別するために番号を振り分けたい
            TargetGenerate(targetId);
            target_now++;
        }
        TargetMove();

    }
    void TargetGenerate(int targetId)
    {
        //Targetを生成する
        GameObject Target = PhotonNetwork.Instantiate("Target", targetPosition[targetId], Quaternion.identity);
        //的の目標を検知
        targetArray[targetId] = Target;
        Target.GetComponent<TargetController>().Id = targetId;
    }


    void TargetMove()
    {
        for(int i=0;i<3;i++)
        {
            if(targetArray[i]!=null)
            {
                targetArray[i].transform.position = targetPosition[i];
            }
            
        }
    }


    /* public override void OnJoinedRoom()
     {
         // ランダムな座標に自身のアバター（ネットワークオブジェクト）を生成する
         var position = new Vector3(0, 0, 0);
         PhotonNetwork.Instantiate("Target", position, Quaternion.identity);
     }*/
}
