using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class BallController : MonoBehaviourPunCallbacks
{
    public int Id= -1;
    public BallManager myManager;
  
    float colideTime = 0;
    public float destroyTime = 0.5f;
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
        if(other.gameObject.tag == "Target")
        {
            colideTime += Time.deltaTime;
            if (colideTime > destroyTime)
            {
                
                    PhotonNetwork.Instantiate("BreakEffect", this.gameObject.transform.position, Quaternion.identity);
                if (photonView.IsMine)
                {
                    myManager.AfterDestory(Id);
                }
                PhotonNetwork.Destroy(this.gameObject);

            }

        }
       
    }

    void OnTriggerExit(Collider other)
    {
        colideTime = 0;
        // 3D同士が離れた瞬間の１回のみ呼び出される処理
    }


  
}