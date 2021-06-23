using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDelete : MonoBehaviour
{
     float timer = 5.0f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, timer);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
