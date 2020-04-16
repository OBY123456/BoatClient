using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MTFrame;
using UnityEngine.UI;
using System;

public class GlobalButtonPanel : BasePanel
{
    public Button[] Buttons;

    public override void InitFind()
    {
        base.InitFind();
        Buttons = FindTool.FindChildNode(transform, "ButtonGroup").GetComponentsInChildren<Button>();
    }

    public override void InitEvent()
    {
        base.InitEvent();

        for (int i = 0; i < Buttons.Length; i++)
        {
            SetButtonClick(Buttons[i], i);
        }
    }

    /// <summary>
    /// 给按钮添加监听事件
    /// </summary>
    /// <param name="button"></param>
    /// <param name="i"></param>
    private void SetButtonClick(Button button,int i)
    {
        button.onClick.AddListener(() => {

            PanelName panelName = (PanelName)Enum.Parse(typeof(PanelName), (i + 1).ToString());
            UdpSclient.Instance.PanelChange((panelName));
            PanelSwitchData data = new PanelSwitchData();
            data.PanelName = panelName.ToString();
            UdpSclient.Instance.SendDataToSever(ParmaterCodes.PanelSwitchData, data);

        });
    }
}
