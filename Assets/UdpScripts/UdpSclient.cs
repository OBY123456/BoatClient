using Proto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NetworkCommonTools;
using System.IO;
using UnityEngine.UI;
using Newtonsoft.Json;
using MTFrame.MTEvent;
using MTFrame;

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
        //Log.Init(new UnityDebug(), false);
        Log.LogIsDebug[Log.LogType.Normal] = true;
        Log.WriteLine("开始");
        UserManager.Instance.LocalUser = new User() { ID = "002", nickname = "aa" };
        //不同端口号和房间名就不会连接
        localClientEngine = new GameLocalClientEngineListener(7878, "Test3");
        localClientEngine.Search();
    }

    // Update is called once per frame
    void Update()
    {
        //测试用
        //if (Input.GetKeyDown(KeyCode.D))
        //{
        //    SendDataToSever(ParmaterCodes.index, "你好，服务器端！");
        //}
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
            case ParmaterCodes.SceneSwitch:
                response.AddParemater((byte)ParmaterCodes.SceneSwitch, JsonConvert.SerializeObject(obj));
                break;
            /* 船体展示页 */
            case ParmaterCodes.BoatRotate:
                response.AddParemater((byte)ParmaterCodes.BoatRotate, JsonConvert.SerializeObject(obj));
                break;
            case ParmaterCodes.Display_PlayVideo:
                response.AddParemater((byte)ParmaterCodes.Display_PlayVideo, JsonConvert.SerializeObject(obj));
                break;
            case ParmaterCodes.Display_VideoControl:
                response.AddParemater((byte)ParmaterCodes.Display_VideoControl, JsonConvert.SerializeObject(obj));
                break;
            /* 模拟航行页 */
            case ParmaterCodes.BoatSpeed:
                response.AddParemater((byte)ParmaterCodes.BoatSpeed, JsonConvert.SerializeObject(obj));
                break;
            case ParmaterCodes.CameraState:
                response.AddParemater((byte)ParmaterCodes.CameraState, JsonConvert.SerializeObject(obj));
                break;
            case ParmaterCodes.DayNightTime:
                response.AddParemater((byte)ParmaterCodes.DayNightTime, JsonConvert.SerializeObject(obj));
                break;
            case ParmaterCodes.OceanLightData:
                response.AddParemater((byte)ParmaterCodes.OceanLightData, JsonConvert.SerializeObject(obj));
                break;
            case ParmaterCodes.OceanWaveSize:
                response.AddParemater((byte)ParmaterCodes.OceanWaveSize, JsonConvert.SerializeObject(obj));
                break;
            case ParmaterCodes.TargetPosition:
                response.AddParemater((byte)ParmaterCodes.TargetPosition, JsonConvert.SerializeObject(obj));
                break;
            case ParmaterCodes.WeatherIntensity:
                response.AddParemater((byte)ParmaterCodes.WeatherIntensity, JsonConvert.SerializeObject(obj));
                break;
            case ParmaterCodes.WeatherType:
                response.AddParemater((byte)ParmaterCodes.WeatherType, JsonConvert.SerializeObject(obj));
                break;
            case ParmaterCodes.TrainModelData:
                response.AddParemater((byte)ParmaterCodes.TrainModelData, JsonConvert.SerializeObject(obj));
                break;
            case ParmaterCodes.PuGuanCameraData:
                response.AddParemater((byte)ParmaterCodes.PuGuanCameraData, JsonConvert.SerializeObject(obj));
                break;
            case ParmaterCodes.AutoDriveData:
                response.AddParemater((byte)ParmaterCodes.AutoDriveData, JsonConvert.SerializeObject(obj));
                break;
            default:
                break;
        }
        localClientEngine?.SendData(response);
    }


    public void SceneChange(SceneName sceneName,PanelName panelName)
    {
        EventParamete eventParamete = new EventParamete();
        eventParamete.AddParameter(panelName);
        EventManager.TriggerEvent(GenericEventEnumType.Message, TransportType.SwitchPanel.ToString(), eventParamete);
        SceneSwitch data = new SceneSwitch();
        data.SceneName = sceneName.ToString();
        SendDataToSever(ParmaterCodes.SceneSwitch, data);
    }

    private void OnDestroy()
    {
        localClientEngine.ShutDown();
    }
}