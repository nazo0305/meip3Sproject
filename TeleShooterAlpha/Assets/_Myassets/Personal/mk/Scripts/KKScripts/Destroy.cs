using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    public GameObject newgameObject;
    public GameObject breakEffect;
    public AudioClip sound1;
    AudioSource audioSource;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //本番ではフラグの受け取りを想定
        if (Time.frameCount > 6000)
        {
            Destroy(newgameObject);
            //エフェクトを発生させる
            GenerateEffect();
        }
    }
    void GenerateEffect()
    {
        //エフェクトを生成する
        GameObject effect = Instantiate(breakEffect) as GameObject;
        //エフェクトが発生する場所を決定する(敵オブジェクトの場所)
        audioSource.PlayOneShot(sound1);
        effect.transform.SetParent(transform.parent.gameObject.transform, false);
        effect.transform.position = gameObject.transform.position;
    }
}