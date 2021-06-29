using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class BallManager: MonoBehaviourPunCallbacks
{
    public int ballId;
    public Vector2[] ballPosition = new Vector2[3];//とりあえず3個
    public int ball_now;
    public GameObject[] ballArray = new GameObject[3] { null,null,null };//とりあえず3個
    public bool[] ballFlag = new bool[3] { false, false, false };

   [SerializeField] GameObject canvas;

    // Start is called before the first frame update
    void Start()
    {
      
        ball_now = 0; //的オブジェクトの数(爆発エフェクト等含む)

    }

    // Update is called once per frame
    void Update()
    {
       
       for(int i=0;i<3;i++)
        {
            //的をそれぞれ識別するために番号を振り分けたい
            if(ballFlag[i]==false)
            {
                ballGenerate(i);
                ball_now++;
            }
            
        }
        
        ballMove();

    }
    void ballGenerate(int ballId)
    {
        //ballを生成する
        GameObject ball = PhotonNetwork.Instantiate("ball", ballPosition[ballId], Quaternion.identity);
        //的の目標を検知
        ballArray[ballId]=ball;
        ballFlag[ballId] = true;
        ball.GetComponent<BallController>().Id = ballId;
        ball.GetComponent<BallController>().myManager = this;


    }


    void ballMove()
    {
        for (int i = 0; i < 3; i++)
        {
            if (ballArray[i] != null)
            {
                ballArray[i].transform.position = ballPosition[i];
            }

        }
    }

    public void AfterDestory(int Id)
    {
        StartCoroutine(FlagDown(Id));
    }



  
    private IEnumerator FlagDown(int Id)
    {
        
        // 3秒間待つ
        yield return new WaitForSeconds(2);
        ballArray[Id] = null;
        ballFlag[Id] = false;

      
    }


    /* public override void OnJoinedRoom()
     {
         // ランダムな座標に自身のアバター（ネットワークオブジェクト）を生成する
         var position = new Vector3(0, 0, 0);
         PhotonNetwork.Instantiate("ball", position, Quaternion.identity);
     }*/
}
