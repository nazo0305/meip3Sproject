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
    public bool[] vanishFlag = new bool[3] { true, true, true };
    Translate Translate;
    [SerializeField] GameObject canvas;
    bool joinFlag=false;
    public ScoreCount scoreCount;
    float[] TimeUntilVanish = new float[3];
    bool[] waitGenerateFlag = new bool[3] { true, true, true };
    // Start is called before the first frame update
    void Start()
    {
      
        ball_now = 0; //的オブジェクトの数(爆発エフェクト等含む)
        Translate = this.gameObject.GetComponent<Translate>();
        scoreCount = this.gameObject.GetComponent<ScoreCount>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckVanish();
        if (joinFlag)
        {
            for (int i = 0; i < 3; i++)
            {
                //的をそれぞれ識別するために番号を振り分けたい
                if (Translate.ball_Flag[i] && !(ballFlag[i]) && waitGenerateFlag[i])
                {
                    ballGenerate(i);
                    ball_now++;
                }

            }

            ballMove();

        }
       
      

    }
    void ballGenerate(int ballId)
    {
        //ballを生成する
        ballPosition = Translate.ball_Position_unity;
        GameObject ball = PhotonNetwork.Instantiate("Ball", ballPosition[ballId], Quaternion.identity);
        //的の目標を検知
        ballArray[ballId]=ball;
        ballFlag[ballId] = true;
        vanishFlag[ballId] = true;
        ball.GetComponent<BallController>().Id = ballId;
        ball.GetComponent<BallController>().myManager = this;


    }


    void ballMove()
    {
        ballPosition = Translate.ball_Position_unity;
        for (int i = 0; i < 3; i++)
        {
            if (ballArray[i] != null)
            {
                ballArray[i].transform.position = ballPosition[i];
            }

        }
    }

    public void AfterDestory(int Id,bool isColision)
    {
        scoreCount.AddScore();
        if(isColision)
        {
            StartCoroutine(FlagDown(Id));
        }
        
        ballArray[Id] = null;
        ballFlag[Id] = false;
    }



  
    private IEnumerator FlagDown(int Id)
    {
        waitGenerateFlag[Id] = false;
        // 3秒間待つ
        yield return new WaitForSeconds(5);
        waitGenerateFlag[Id] = true;


    }

    void CheckVanish()
    {
        for (int i = 0; i < 3; i++)
        {
            if (ballFlag[i] && !(Translate.target_Flag[i]))
            {
                TimeUntilVanish[i] += Time.deltaTime;
                Debug.Log("vanish");
                if (TimeUntilVanish[i] > 0.1f)
                {

                    if (vanishFlag[i])
                    {
                        ballArray[i].GetComponent<BallController>().Dest();
                    }
                    AfterDestory(i,false);
                    vanishFlag[i] = false;

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


    /* public override void OnJoinedRoom()
     {
         // ランダムな座標に自身のアバター（ネットワークオブジェクト）を生成する
         var position = new Vector3(0, 0, 0);
         PhotonNetwork.Instantiate("ball", position, Quaternion.identity);
     }*/
}
