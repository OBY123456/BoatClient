using Proto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NetworkCommonTools;
using System.IO;
using UnityEngine.UI;
using Newtonsoft.Json;

//****Udp客户端****
//****数据接收在GameHandle脚本接收****
//****要在PlayerSettings->otherSettings里面将Api Compatibility Level设置为.net4.x  如果没有则无法使用
public class UdpSclient : MonoBehaviour
{
    public static UdpSclient Instance;
    public GameLocalClientEngineListener localClientEngine;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //打印debug信息，如果不需要可以注释掉
        Log.Init(new UnityDebug(), false);
        Log.LogIsDebug[Log.LogType.Normal] = true;
        Log.WriteLine("开始");
        UserManager.Instance.LocalUser = new User() { ID = "002", nickname = "aa" };
        //不同端口号和房间名就不会连接
        localClientEngine = new GameLocalClientEngineListener(9999, "Test4");
        localClientEngine.Search();
    }

    // Update is called once per frame
    void Update()
    {
        //测试用
        if (Input.GetKeyDown(KeyCode.D))
        {
            SendDataToSever(ParmaterCodes.index, "你好，服务器端！");
        }
    }

    /// <summary>
    /// 发送data给服务端
    /// </summary>
    /// <param name="parmaterCodes"></param>
    /// <param name="obj"></param>
    public void SendDataToSever(ParmaterCodes parmaterCodes, object obj)
    {
        OperationResponse response = OperationResponseExtend.GetOperationResponse((byte)OperateCodes.Game);
        switch (parmaterCodes)
        {
            case ParmaterCodes.index:
                response.AddParemater((byte)ParmaterCodes.index, obj);
                Debug.Log("发送信息给服务器端:" + obj);
                break;
            case ParmaterCodes.People:
                response.AddParemater((byte)ParmaterCodes.People, JsonConvert.SerializeObject(obj));
                break;
            default:
                break;
        }
        localClientEngine?.SendData(response);
    }

    private void OnDestroy()
    {
        localClientEngine.ShutDown();
    }
}