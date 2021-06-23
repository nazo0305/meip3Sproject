using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class BallGenerator : MonoBehaviour
   
{

    [SerializeField] GameObject BallPrefab;
    public float speed = 200f;
    public Vector3 InitialPosition= new Vector3(0,0,0);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            GameObject ball = PhotonNetwork.Instantiate("Ball",Vector3.zero, Quaternion.identity) as GameObject;
            Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 direction = r.direction;
            ball.transform.position = InitialPosition;
            ball.GetComponent<Rigidbody>().AddForce(direction.normalized * speed);
        }
    }
}
