
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Translate : MonoBehaviour
{
    public static Vector2 position;
    public static Vector2 CameraVec;
    public static Vector2 origin;
    public static Vector2 reduction_rate;
    private static Vector2[] ball_Position = new Vector2[3];
    private static Vector2[] target_Position = new Vector2[3];
    public static Vector2[] ball_Position_unity = new Vector2[3];
    public static Vector2[] target_Position_unity = new Vector2[3];
    public bool[] ball_Flag = { false, false, false };
    public bool[] target_Flag = { false, false, false };
    GameObject UDPReciever;
    UDP UDP;
    TranslateToValidRange TranslateToValidRange;

    // Start is called before the first frame update
    void Start()
    {
        UDPReciever = GameObject.Find("UDPReciever");
        UDP = UDPReciever.GetComponent<UDP>();
        TranslateToValidRange = this.GetComponent<TranslateToValidRange>();
        origin = TranslateToValidRange.origin;
        reduction_rate = TranslateToValidRange.reduction_rate;
        Debug.Log("rudection" + reduction_rate[0]);
    }

    // Update is called once per frame
    void Update()
    {
        //値読み込み
        ball_Position = TranslateToValidRange.ball_Position;
        target_Position = TranslateToValidRange.target_Position;
        ball_Flag = UDP.ball_infoFlag;
        target_Flag = UDP.target_infoFlag;
        //変換
        for (int i = 0; i < 3; i++)
        {
            ball_Position_unity[i] = TransformToValidRange(origin, reduction_rate, ball_Position[i]);
            target_Position_unity[i] = TransformToValidRange(origin, reduction_rate, target_Position[i]);
         }

    //ball_Position_unity = TransformToValidRange(origin, reduction_rate, FixTransform(ball_Position));
    //target_Position_unity = TransformToValidRange(origin, reduction_rate, FixTransform(target_Position));

}

public Vector2 FixTransform(Vector2 vector)
{
    Vector2 position;
    float x_new = 0, y_new = 0;
    float x, y;
    x = vector[0];
    y = vector[1];
    x_new = (x - 320) / 100;
    y_new = (-y + 240) / 100;
    position = new Vector2(x_new, y_new);
    return position;
}

public Vector2 TransformToValidRange(Vector2 ValidOrigin, Vector2 reduction_rate, Vector2 raw_vector)
{
    float x_newVect;
    float y_newVect;
    Vector2 error = new Vector2(ValidOrigin[0] - 320, ValidOrigin[1] - 240);
    //カメラ座標




    x_newVect = -1*((raw_vector[0] - error[0]) - 320) / (reduction_rate[0] * 100);
    y_newVect = -1*(-(raw_vector[1] - error[1]) + 240) / (reduction_rate[1] * 100);
    Vector2 position = new Vector2(x_newVect, y_newVect);
    return position;
}



}