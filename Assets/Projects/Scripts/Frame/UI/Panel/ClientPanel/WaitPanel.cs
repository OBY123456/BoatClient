using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MTFrame;
using UnityEngine.UI;

public class WaitPanel : BasePanel
{
    public Button DisplayButton, xunlianButton;

    public override void InitFind()
    {
        base.InitFind();
        DisplayButton = FindTool.FindChildComponent<Button>(transform, "DisplayButton");
        xunlianButton = FindTool.FindChildComponent<Button>(transform, "xunlianButton");
    }

    public override void InitEvent()
    {
        base.InitEvent();
        DisplayButton.onClick.AddListener(() => {
            UdpSclient.Instance.SceneChange(SceneName.DisplayScene,PanelName.DisplayPanel);
        });

        xunlianButton.onClick.AddListener(() => {
            UdpSclient.Instance.SceneChange(SceneName.Sailing,PanelName.SailingPanel);
        });
    }

    public override void Open()
    {
        base.Open();
    }

    public override void Hide()
    {
        base.Hide();
    }


}