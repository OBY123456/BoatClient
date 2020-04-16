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
    /// 切换场景或页面类型
    /// </summary>
    PanelSwitchData,

    /*船体展示页传输数据类型枚举*/
    BoatRotateX,

    BoatRotateY,

    BoatRotateZ,
}
