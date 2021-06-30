using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class TranslateToValidRange : MonoBehaviour
{
    private static float shrink_x = 6.4F;
    private static float shrink_y = 4.8F;
    private static Vector2 reduction_rate;
    private static Vector2 origin;
    public static Vector2 position, raw_vector;
    public static Vector2[] ball_Position = new Vector2[3];
    public static Vector2[] target_Position = new Vector2[3];
    public static Vector2 upperleft, lowerleft, lowerright, upperright;
    public Vector2[,] Corner = new Vector2[3,4];
    GameObject Canvas;
    GameObject UDPReciever;
    UDP UDP;

    // Start is called before the first frame update
    void Start()
    {
        //変数受け取りを追加
        Canvas = GameObject.Find("Canvas");
        float[] points = GetAnglePoints.point;
        upperleft = new Vector2(points[0],points[1]);
        lowerleft = new Vector2(points[2],points[3]);
        lowerright = new Vector2(points[4],points[5]);
        upperright = new Vector2(points[6],points[7]);

        UDPReciever = GameObject.Find("UDPReciever");
        UDP = UDPReciever.GetComponent<UDP>();
       
        //座標変換を実行
        origin = NewOrigin(upperleft,lowerleft,lowerright,upperright);
        reduction_rate = NewAxis(upperleft,lowerleft,lowerright,upperright);
    }

    // Update is called once per frame
    void Update()
    {
        //変数受け取り
        float [,] ball_infoPosition = UDP.ball_infoPosition;
        float [,] target_infoPosition = UDP.target_infoPosition;
        for(int i=0;i<3;i++){
            for(int j=0;j<8;j++){
                Corner[i,j/2][j%2] = target_infoPosition[i,j]; 
            }
        }
        for(int i=0;i<3;i++){
            target_Position[i] = NewOrigin(Corner[i,0],Corner[i,1],Corner[i,2],Corner[i,3]);
        }
        for(int i=0;i<3;i++){
            for(int j=0;j<2;j++){
                ball_Position[i][j] = ball_infoPosition[i,j]; 
            }
        }

        //座標変換
        position = TransformToValidRange(origin,reduction_rate,raw_vector);
    }

    public Vector2 NewAxis(Vector2 upperleft,Vector2 lowerleft,Vector2 lowerright,Vector2 upperright){
        float x_reduction_rate, y_reduction_rate;
        Vector2  reduction_rate;
        x_reduction_rate = (upperright[0]-upperleft[0])/shrink_x;
        y_reduction_rate = (upperleft[1]-lowerleft[1])/shrink_y;

        reduction_rate= new Vector2(x_reduction_rate, y_reduction_rate);
        return reduction_rate;
    }
    public Vector2 NewOrigin(Vector2 upperleft,Vector2 lowerleft,Vector2 lowerright,Vector2 upperright){
        float origin_x, origin_y;
        Vector2 origin;
        origin_x = (upperleft[0]+lowerleft[0]+lowerright[0]+upperright[0])/4;
        origin_y = (upperleft[1]+lowerleft[1]+lowerright[1]+upperright[1])/4;
        
        origin = new Vector2(origin_x, origin_y);
        return origin;
    }

    public Vector2 TransformToValidRange(Vector2 origin,Vector2 reduction_rate,Vector2 raw_vector){
            float x_new, y_new;
            x_new = (raw_vector[0]-origin[0])/reduction_rate[0];
            y_new = (raw_vector[1]-origin[1])/reduction_rate[1];
            Vector2 position = new Vector2(x_new, y_new);
            return position;
    }
}

