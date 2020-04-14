using NetworkCommonTools;
using Proto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using MTFrame.MTEvent;
using System;
using Newtonsoft.Json;

//****Udp服务器端****
//****数据接收在GameHandle脚本接收****
//****要在PlayerSettings->otherSettings里面将Api Compatibility Level设置为.net4.x 如果没有则无法使用

public class UdpSeverLink : MonoBehaviour
{
    public static UdpSeverLink Instance;
    public GameLocalServerEngineListener localServerEngine;
    public Transform Cube;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        //打印debug信息，如果不需要可以注释掉
        Log.Init(new UnityDebug(), true);
        Log.LogIsDebug[Log.LogType.Normal] = true;
        Log.WriteLine("开始");

        UserManager.Instance.LocalUser = new User() { ID = "001", nickname = "xxx" };
        //设置房间信息
        RoomManager.Instance.LocalRoom = new Room(UserManager.Instance.LocalUser, 4);
        RoomManager.Instance.LocalRoom.RoomInfo.RoomName = "房间001";

        localServerEngine = new GameLocalServerEngineListener(9999, "Test4");
        localServerEngine.Creat();

        EventManager.AddListener(GenericEventEnumType.Message, ParmaterCodes.People.ToString(), callback);
    }

    private void callback(EventParamete parameteData)
    {
        if(parameteData.EvendName == ParmaterCodes.People.ToString())
        {
            string data = parameteData.GetParameter<string>()[0];
            RotateData rotateData = new RotateData();
            rotateData = Newtonsoft.Json.JsonConvert.DeserializeObject<RotateData>(data);
            Cube.localEulerAngles = new Vector3(rotateData.Rotate_x, rotateData.Rotate_y, rotateData.Rotate_z);
        }
    }

    private void Update()
    {
        //测试用
        if(Input.GetKeyDown(KeyCode.A))
        {
            SendDataToClient(ParmaterCodes.index,"你好，客户端！");
        }
    }

    private void OnDestroy()
    {
        localServerEngine.ShutDown();
        EventManager.RemoveListener(GenericEventEnumType.Message, ParmaterCodes.People.ToString(), callback);
    }

    public void SendDataToClient(ParmaterCodes parmaterCodes, object obj)
    {
        OperationResponse response = OperationResponseExtend.GetOperationResponse((byte)OperateCodes.Game);
        switch (parmaterCodes)
        {
            case ParmaterCodes.index:
                response.AddParemater((byte)ParmaterCodes.index, obj);
                Debug.Log("发送信息给客户端:" + obj);
                break;
            case ParmaterCodes.People:
                response.AddParemater((byte)ParmaterCodes.People, JsonConvert.SerializeObject(obj));
                break;
            default:
                break;
        }
        localServerEngine?.SendData(response);
    }
}


