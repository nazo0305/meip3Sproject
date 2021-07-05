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
    public GameObject[] targetArray= new GameObject[3] { null, null, null };//とりあえず3個
    public bool[] targetFlag = new bool[3] { false, false, false };
    public bool[] destroyFlag = new bool[3] { false, false, false };
    Translate Translate;
    [SerializeField]GameObject canvas;
    bool joinFlag = false;
    float[] TimeUntilVanish=new float[3];

    // Start is called before the first frame update
    void Start()
    {
        count = 0;     //認識されている四角形
        target_now = 0; //的オブジェクトの数(爆発エフェクト等含む)
        Translate = this.gameObject.GetComponent < Translate > ();

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

        CheckVanish();
        if (joinFlag)
        {
            for (int i = 0; i < 3; i++)
            {

                //的をそれぞれ識別するために番号を振り分けたい
                if (Translate.target_Flag[i] && !(targetFlag[i]))
                {
                    TargetGenerate(i);
                    target_now++;
                }

            }

            TargetMove();
        }


    }
    void TargetGenerate(int targetId)
    {
        targetPosition = Translate.target_Position_unity;
        //Targetを生成する
        GameObject Target = PhotonNetwork.Instantiate("Target", targetPosition[targetId], Quaternion.identity);
        //的の目標を検知
        targetArray[targetId] = Target;
        //targetFlag[targetId] = true;
        Target.GetComponent<TargetController>().Id = targetId;
        Target.GetComponent<TargetController>().myManager = this;
        targetFlag[targetId] = true;
        destroyFlag[targetId] = false;
    }


    void TargetMove()
    {
        targetPosition = Translate.target_Position_unity;
        for (int i=0;i<3;i++)
        {
            if(targetArray[i]!=null)
            {
                targetArray[i].transform.position = targetPosition[i];
            }
            
        }
    }

    public void AfterDestory(int Id)
    {
        destroyFlag[Id] = true;
      
        StartCoroutine(FlagDown(Id));
    }




    private IEnumerator FlagDown(int Id)
    {

        // 3秒間待つ
        yield return new WaitForSeconds(2);
        destroyFlag[Id] = true;
        targetArray[Id] = null;
        targetFlag[Id] = false;


    }


    /* public override void OnJoinedRoom()
     {
         // ランダムな座標に自身のアバター（ネットワークオブジェクト）を生成する
         var position = new Vector3(0, 0, 0);
         PhotonNetwork.Instantiate("Target", position, Quaternion.identity);
     }*/

    void CheckVanish()
    {
        for (int i = 0;i< 3;i++)
        {
            if(targetFlag[i] && !(Translate.target_Flag[i]))
            {
                TimeUntilVanish[i] += Time.deltaTime;
                if(TimeUntilVanish[i]>1f)
                {
                    AfterDestory(i);
                }
            }
            else
            {
                TimeUntilVanish[i] = 0;
            }
        }
    }

    public override void OnConnectedToMaster()
    {
        joinFlag = true;
    }
}
