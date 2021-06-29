using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coliderManager : MonoBehaviour
{

    float colideTime=0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay(Collider other)
    {
        colideTime += Time.deltaTime;
        if(colideTime>1f)
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);

        }
    }

    void OnTriggerExit(Collider other)
    {
        colideTime = 0;
        // 3D同士が離れた瞬間の１回のみ呼び出される処理
    }

}
