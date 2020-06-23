using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MTFrame;
using UnityEngine.UI;

public class PuGuanPanel : BasePanel
{
    public CanvasGroup mask;
    public BlueButton SwitchButton;

    public override void InitFind()
    {
        base.InitFind();
        mask = FindTool.FindChildComponent<CanvasGroup>(transform, "Button/mask");
        SwitchButton = FindTool.FindChildComponent<BlueButton>(transform, "Button/SwitchButton");
    }

    public override void InitEvent()
    {
        base.InitEvent();
        SwitchButton.button.onClick.AddListener(() => {
            PuGuanSwitchData data = new PuGuanSwitchData();
            if(SwitchButton.state == BlueButton.State.write)
            {
                SwitchButton.text.color = SwitchButton.blue;
                SwitchButton.state = BlueButton.State.blue;
                data.state = PuGuanSwitch.Open.ToString();
            }
            else
            {
                SwitchButton.text.color = Color.white;
                SwitchButton.state = BlueButton.State.write;
                data.state = PuGuanSwitch.Close.ToString();
            }
            UdpSclient.Instance.SendDataToSever(ParmaterCodes.PuGuanSwitchData, data);
        });
    }

    public override void Open()
    {
        MaskHide();
    }

    public override void Hide()
    {
        MaskOpen();
    }

    private void MaskOpen()
    {
        mask.alpha = 1;
        mask.blocksRaycasts = true;
        IsOpen = true;
    }

    private void MaskHide()
    {
        mask.alpha = 0;
        mask.blocksRaycasts = false;
        IsOpen = false;
    }

    public void Reset()
    {
        SwitchButton.Reset();
        MaskOpen();
    }
}
