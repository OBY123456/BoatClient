using System;
using System.Collections.Generic;
using System.Net;
using LiteNetLib;
using Proto;
using NetworkCommonTools.LiteNetLibEngine;

public class GameHandle : HandleBase
{
    MTFrame.MTEvent.EventParamete eventParamete = new MTFrame.MTEvent.EventParamete();
    public override byte OperateHandleCode => (byte)OperateCodes.Game;

    public override void OnReceiveProcess(INetEngine netEngine, NetPeer netPeer, OperationResponse operation)
    {
        //这里获取相应数据类型的数据，这里获取的string类型数据
        string people = operation.GetParemater<string>(ParmaterCodes.People);
        //当传输的数据中可能出现0的时候，需要自定义一个类来存储这个0
        if (people != null)
        {
            SentDataToState(people, ParmaterCodes.People);
            return;
        }

        string index = operation.GetParemater<string>(ParmaterCodes.index);
        //引用类型判断是否为null
        if (index != null)
        {
            SentDataToState(index, ParmaterCodes.index);
            return;
        }
    }

    public override void OnUnconnectedRequestProcess(INetEngine netEngine, IPEndPoint endPoint, OperationResponse operation)
    {

    }

    public override void OnUnconnectedResponseProcess(INetEngine netEngine, IPEndPoint endPoint, OperationResponse operation)
    {

    }

    private void SentDataToState(string msg, ParmaterCodes parmater)
    {
        QueueData queueData = new QueueData();
        queueData.msg = msg;
        queueData.parmaterCodes = parmater;
        eventParamete.AddParameter(queueData);
        UnityEngine.Debug.Log(msg);
        EventManager.TriggerEvent(MTFrame.MTEvent.GenericEventEnumType.Message, TransportType.UdpToState.ToString(), eventParamete);
    }
}