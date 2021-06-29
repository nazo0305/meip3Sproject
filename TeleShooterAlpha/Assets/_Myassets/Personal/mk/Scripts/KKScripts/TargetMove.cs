using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMove : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Test用コード
        //画像処理・座標変換で処理した座標を取得するコードをここに追加
        // transformを取得
        Transform myTransform = this.transform;

        // 座標を取得
        Vector3 pos = myTransform.position;
        pos.x += 0.01f;    // x座標へ0.01加算
        pos.y += 0.01f;    // y座標へ0.01加算
        pos.z += 0.01f;    // z座標へ0.01加算

        myTransform.position = pos;  // 座標を設定
    }
}
