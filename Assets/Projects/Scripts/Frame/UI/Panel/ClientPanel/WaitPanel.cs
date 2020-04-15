using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MTFrame;
using UnityEngine.UI;

public class WaitPanel : BasePanel
{
    public Button button;

    public override void InitFind()
    {
        base.InitFind();
        button = FindTool.FindChildComponent<Button>(transform, "bg");
    }

    public override void InitEvent()
    {
        base.InitEvent();
        button.onClick.AddListener(() => {

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