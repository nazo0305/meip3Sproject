using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetAnglePoints : MonoBehaviour
{
    [SerializeField]InputField inputField;
    public static float[] point = new float[8] {-320,240,-320,-240,320,-240,320,240 };
    // Start is called before the first frame update
   /* void Start()
    {
        inputField = GameObject.Find("InputField").GetComponent<InputField>();
    }

    public void GetInputName(){
        string name = inputField.text;
        string[] arr = name.Split(' ');
        for(int i=0;i<8;i++){
            point[i] = float.Parse(arr[i]);
        }

 
        //入力フォームのテキストを空にする
        inputField.text = "";
        //this.gameObject.SetActive(false);
    }*/
}
