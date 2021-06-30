using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Translate : MonoBehaviour
{
    public Vector2 position;
    public Vector2 CameraVec;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
            position = FixTransform(CameraVec);
    }

    public Vector2 FixTransform(Vector2 vector){
        Vector2 position;
        float x_new=0,y_new=0;
        float x,y;
        x = vector[0];
        y = vector[1];
        x_new = (x-320)/100;
        y_new = (-y+240)/100;
        position = new Vector2(x_new,y_new);
        return position;
    }
}
