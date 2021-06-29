using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColParticle : MonoBehaviour
{
    // Start is called before the first frame update

    //変数の定義
    private ParticleSystem particle;

    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {

    }

    //衝突が発生した場合に実行される
    void OnTriggerEnter(Collider other)
    {
            particle.Play();
    }
}