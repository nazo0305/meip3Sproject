using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCreator : MonoBehaviour
{
    public GameObject newTargetObject;
    public int count;
    public int target_now;

    [SerializeField]
    GameObject canvas;

    // Start is called before the first frame update
    void Start()
    {
        count = 0;     //認識されている四角形
        target_now = 0;　//的オブジェクトの数(爆発エフェクト等含む)
    }

    // Update is called once per frame
    void Update()
    {
        //必要Target数を取得
        count = 1;
        //Target数を取得
        target_now = this.transform.childCount - 1;
        //目標の数が足りていなければTargetを生成
        if (Time.frameCount > 3000 && target_now < count)
        {
            //的をそれぞれ識別するために番号を振り分けたい
            TargetCreate();
            target_now++;
        }
    }

    void TargetCreate()
    {
        //Targetを生成する
        GameObject effect = Instantiate(newTargetObject) as GameObject;
        //的の目標を検知

        //Targetが発生する場所を決定する(敵オブジェクトの場所)(現時点ではcanvasの座標)
        effect.transform.SetParent(canvas.transform, false);
        effect.transform.position = gameObject.transform.position;
    }
}
