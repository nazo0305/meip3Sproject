using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class BallManager: MonoBehaviourPunCallbacks
{
    public int ballId;
    public Vector2[] ballPosition = new Vector2[3];//とりあえず3個
    public int count;
    public int ball_now;
    public GameObject[] ballArray = new GameObject[3];//とりあえず3個

    [SerializeField] GameObject canvas;

    // Start is called before the first frame update
    void Start()
    {
        count = 0;     //認識されている四角形
        ball_now = 0; //的オブジェクトの数(爆発エフェクト等含む)

    }

    // Update is called once per frame
    void Update()
    {
        //必要ball数を取得
        count = 1;
        //ball数を取得
        ball_now = this.transform.childCount - 1;
        //目標の数が足りていなければballを生成
        if (Time.frameCount > 3000 && ball_now < count)
        {
            //的をそれぞれ識別するために番号を振り分けたい
            ballGenerate(ballId);
            ball_now++;
        }
        ballMove();

    }
    void ballGenerate(int ballId)
    {
        //ballを生成する
        GameObject ball = PhotonNetwork.Instantiate("ball", ballPosition[ballId], Quaternion.identity);
        //的の目標を検知
        ballArray[ballId] = ball;
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


    /* public override void OnJoinedRoom()
     {
         // ランダムな座標に自身のアバター（ネットワークオブジェクト）を生成する
         var position = new Vector3(0, 0, 0);
         PhotonNetwork.Instantiate("ball", position, Quaternion.identity);
     }*/
}
