using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class UDP : MonoBehaviour
{

    static UdpClient udp;
    IPEndPoint remoteEP = null;
    public float[,] ball_infoPosition = new float[3, 2];
    public bool[] ball_infoFlag = { false, false, false };
    public float[,] target_infoPosition = new float[3, 8];
    public bool[] target_infoFlag = { false, false, false };
    public int ballcount;
    public int targetcount;
    // Use this for initialization
    void Start()
    {
        int LOCA_LPORT = 50007;

        udp = new UdpClient(LOCA_LPORT);
        udp.Client.ReceiveTimeout = 200;
        Debug.Log("target_infoFlag=" + target_infoFlag[0]);
    }

    // Update is called once per frame
    void Update()
    {
        int i = 0;
        IPEndPoint remoteEP = null;
        byte[] data = udp.Receive(ref remoteEP);
        string text = Encoding.UTF8.GetString(data);
        //多分Replace処理は分けないと死
        string[] arr = text.Split('\n');
        //的の処理
        bool[] tInfoFlag= { false, false, false };
        bool[] bInfoFlag = { false, false, false };
        targetcount = int.Parse(arr[i]);
        i++;
        for (int j = 0; j < targetcount; j++)
        {
            string[] arr_posi = arr[i].Split(' ');
            int target_infoID = int.Parse(arr_posi[0]);
            target_infoFlag[target_infoID] = true;
            tInfoFlag[target_infoID] = true;
            Debug.Log("target_infoFlag=" + target_infoID + target_infoFlag[target_infoID]);
            for (int k = 1; k < 9; k++)
            {
                target_infoPosition[target_infoID, k - 1] = float.Parse(arr_posi[k]);
            }
            i++;
        }
        for (int j = 0; j < 3; j++)
        {
            if(!(tInfoFlag[j]))
            {
                target_infoFlag[j] = false;
            }
        }

        //ボールの処理
        ballcount = int.Parse(arr[i]);
        i++;
        for (int j = 0; j < ballcount; j++)
        {
            
            string[] arr_posi = arr[i].Split(' ');
            int ball_infoID = int.Parse(arr_posi[0]);
            ball_infoFlag[ball_infoID] = true;
            bInfoFlag[ball_infoID] = true;
            for (int k = 1; k < 3; k++)
            {
                ball_infoPosition[ball_infoID, k - 1] = float.Parse(arr_posi[k]);
            }
            
            i++;
        }
        for (int j = 0; j < 3; j++)
        {
            if (!(bInfoFlag[j]))
            {
                ball_infoFlag[j] = false;
            }
        }

      
      
    }
}