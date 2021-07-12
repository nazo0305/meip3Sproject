using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TargetController : MonoBehaviourPunCallbacks
{
    public int Id=-1;
    public TargetManager myManager;
    float colideTime = 0;
    public float destroyTime;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            var input = new Vector3(Input.GetAxis("Horizontal")*-1, Input.GetAxis("Vertical"), 0f);
            transform.Translate(6f * Time.deltaTime * input.normalized);
        }
    }
    void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag=="Ball")
        {
            colideTime += Time.deltaTime;
            if (colideTime > destroyTime)
            {
                
                if (photonView.IsMine)
                {
                    myManager.scoreCount.AddScore();
                    myManager.AfterDestory(Id,true);
                    PhotonNetwork.Instantiate("BreakEffect", this.gameObject.transform.position, Quaternion.identity);

                }
                //PhotonNetwork.Destroy(this.gameObject);
                Dest();


            }

        }
        
    }

    public void Dest()
    {
        Debug.Log("delete");
        PhotonNetwork.Destroy(this.gameObject);
    }

    void OnTriggerExit(Collider other)
    {
        colideTime = 0;
        // 3D同士が離れた瞬間の１回のみ呼び出される処理
    }
}
