using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MTFrame;
using MTFrame.MTEvent;
using System;

public class QueueData
{
    public string msg;

    public ParmaterCodes parmaterCodes;
}

public enum TransportType
{
    UdpToState,
    SwitchPanel,
}

public enum PanelName
{
    WaitPanel,
}

public class UdpState : BaseState
{
    //注意state一定要在get里面监听事件，没有的话就写成下面样子
    //这里一般用来监听Panel切换
    private Queue<QueueData> GetVs = new Queue<QueueData>();
    public override string[] ListenerMessageID
    {
        get
        {
            return new string[]
            {
                //事件名string类型
                TransportType.UdpToState.ToString(),
                TransportType.SwitchPanel.ToString(),
            };
        }
        set { }
    }

    public override void OnListenerMessage(EventParamete parameteData)
    {

        //接收监听事件的数据，然后用switch判断做处理

        //除此之外，也可以在这里监听UDP传输的数据，但是接收的数据是子线程数据，要通过队列接收，
        //然后在update转换成主线程数据，才能对数据进行处理

        if(parameteData.EvendName == TransportType.UdpToState.ToString())
        {
            QueueData queueData = parameteData.GetParameter<QueueData>()[0];
            GetVs.Enqueue(queueData);
        }

        if(parameteData.EvendName == TransportType.SwitchPanel.ToString())
        {
            PanelName panelName = parameteData.GetParameter<PanelName>()[0];
            switch (panelName)
            {
                case PanelName.WaitPanel:
                    CurrentTask.ChangeTask(new WaitTask(this));
                    break;
                default:
                    break;
            }
        }
    }

    public override void Enter()
    {
        base.Enter();
        CurrentTask.ChangeTask(new WaitTask(this));
        EventManager.AddUpdateListener(UpdateEventEnumType.Update,"OnUpdate",Onupdate);
    }

    private void Onupdate(float timeProcess)
    {
        //数据在这里转换
        lock(GetVs)
        {
            if(GetVs.Count > 0)
            {
                QueueData st = GetVs.Dequeue();
                Debug.Log("状态类里接收到的数据：" + st.msg);
                EventParamete eventParamete = new EventParamete();
                eventParamete.AddParameter(st.msg);
                switch (st.parmaterCodes)
                {
                    case ParmaterCodes.index:
                        EventManager.TriggerEvent(GenericEventEnumType.Message, ParmaterCodes.index.ToString(), eventParamete);
                        break;
                    case ParmaterCodes.People:
                        EventManager.TriggerEvent(GenericEventEnumType.Message, ParmaterCodes.People.ToString(), eventParamete);
                        break;
                    default:
                        break;
                }

                
                //在这里进行switch对数据进行处理
            }
        }
    }
}
