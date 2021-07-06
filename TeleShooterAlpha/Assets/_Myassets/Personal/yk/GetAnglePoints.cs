using UnityEngine;
using UnityEngine.UI;

public class GetAnglePoints : MonoBehaviour
{
    [SerializeField]InputField inputField;
    public static float[] points = new float[8] {0,0,0,480,640,480,640,0};
    // Start is called before the first frame update
    /*void Start()
    {
        inputField = GameObject.Find("InputField").GetComponent<InputField>();
    }
    //初期値は、カメラ座標で入力。
    //(0,0), (0,480), (640,480), (640, 0) 
    public void GetInputName(){
        string name = inputField.text;
        string[] arr = name.split(" ");
        for(int i=0;i<8;i++){
            point[i] = float.Parse(arr[i]);
        }

 
        //入力フォームのテキストを空にする
        inputField.text = "";
    }*/
}