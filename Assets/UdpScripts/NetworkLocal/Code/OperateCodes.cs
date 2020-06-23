using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 只会接收和发送对应枚举的数据
/// </summary>
public enum OperateCodes : byte
{
    Game
}

/// <summary>
/// 只会接收和发送对应枚举的数据
/// </summary>
public enum ParmaterCodes : byte
{
    /// <summary>
    /// 字符串类型
    /// </summary>
    index,

    /// <summary>
    /// 切换场景
    /// </summary>
    SceneSwitch,

    /*船体展示页传输数据类型枚举*/
    /// <summary>
    /// 船体展示页1
    /// </summary>
    BoatRotate,
    /// <summary>
    /// 船体展示页2
    /// </summary>
    Display_PlayVideo,
    /// <summary>
    /// 船体展示页3
    /// </summary>
    Display_VideoControl,

    /*模拟航行页传输数据类型枚举*/
    /// <summary>
    /// 模拟航行页1
    /// </summary>
    WeatherType,
    /// <summary>
    /// 模拟航行页2
    /// </summary>
    WeatherIntensity,
    /// <summary>
    /// 模拟航行页3
    /// </summary>
    DayNightTime,
    /// <summary>
    /// 模拟航行页4
    /// </summary>
    BoatSpeed,
    /// <summary>
    /// 模拟航行页5
    /// </summary>
    OceanWaveSize,
    /// <summary>
    /// 模拟航行页6
    /// </summary>
    OceanLightData,
    /// <summary>
    /// 模拟航行页7
    /// </summary>
    CameraState,
    /// <summary>
    /// 模拟航行页8
    /// </summary>
    TargetPosition,
    /// <summary>
    /// 模拟航行页9
    /// </summary>
    TrainModelData,
    /// <summary>
    /// 模拟航行页10
    /// </summary>
    PuGuanCameraData,
    /// <summary>
    /// 模拟航行页11
    /// </summary>
    AutoDriveData,
    /// <summary>
    /// 模拟航行页12
    /// </summary>
    DriveTurnData,
    /// <summary>
    /// 模拟航行页13
    /// </summary>
    DriveSpeed,
    /// <summary>
    /// 模拟航行页14
    /// </summary>
    PuGuanSwitchData,
    /// <summary>
    /// 模拟航行页15
    /// </summary>
    TurnTableData,
    /// <summary>
    /// 模拟航行页16
    /// </summary>
    CraneHandData,
    /// <summary>
    /// 模拟航行页17
    /// </summary>
    HookData,
}
