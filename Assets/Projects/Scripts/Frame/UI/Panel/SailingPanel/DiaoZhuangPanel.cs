using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MTFrame;
using UnityEngine.UI;

public class DiaoZhuangPanel : BasePanel
{
    public Slider TurnTableSlider, CraneHandSlider;
    public Text TurnTableText, CraneHandText;
    public BlueButton HookDown, HookUp;
    public Button StopButton, ResetButton;

    public override void InitFind()
    {
        base.InitFind();
        TurnTableSlider = FindTool.FindChildComponent<Slider>(transform, "bg/TurnTable/Slider");
        CraneHandSlider = FindTool.FindChildComponent<Slider>(transform, "bg/CraneHand/Slider");
        TurnTableText = FindTool.FindChildComponent<Text>(transform, "bg/TurnTable/Text (1)");
        CraneHandText = FindTool.FindChildComponent<Text>(transform, "bg/CraneHand/Text (1)");
        HookDown = FindTool.FindChildComponent<BlueButton>(transform, "bg/HookButtonGroup/Hook-Down");
        HookUp = FindTool.FindChildComponent<BlueButton>(transform, "bg/HookButtonGroup/Hook-Up");
        StopButton = FindTool.FindChildComponent<Button>(transform, "bg/ButtonGroup/Stop");
        ResetButton = FindTool.FindChildComponent<Button>(transform, "bg/ButtonGroup/Reset");
    }

    public override void InitEvent()
    {
        base.InitEvent();
        HookDown.button.onClick.AddListener(() => {
            HookData hookData = new HookData();
            if(HookDown.state == BlueButton.State.write)
            {
                HookDown.text.color = HookDown.blue;
                HookDown.state = BlueButton.State.blue;
                hookData.state = HookState.Down.ToString();
            }
            else
            {
                HookDown.text.color = Color.white;
                HookDown.state = BlueButton.State.write;
                hookData.state = HookState.Stop.ToString();
            }
            HookUp.Reset();
            UdpSclient.Instance.SendDataToSever(ParmaterCodes.HookData, hookData);
        });

        HookUp.button.onClick.AddListener(() => {
            HookData hookData = new HookData();
            if (HookUp.state == BlueButton.State.write)
            {
                HookUp.text.color = HookUp.blue;
                HookUp.state = BlueButton.State.blue;
                hookData.state = HookState.Up.ToString();
            }
            else
            {
                HookUp.text.color = Color.white;
                HookUp.state = BlueButton.State.write;
                hookData.state = HookState.Stop.ToString();
            }
            HookDown.Reset();
            UdpSclient.Instance.SendDataToSever(ParmaterCodes.HookData, hookData);
        });

        StopButton.onClick.AddListener(() => {
            HookDown.Reset();
            HookUp.Reset();
            HookData hookData = new HookData();
            hookData.state = HookState.Stop.ToString();
            UdpSclient.Instance.SendDataToSever(ParmaterCodes.HookData, hookData);
        });

        ResetButton.onClick.AddListener(() => {
            Reset();
            HookData hookData = new HookData();
            hookData.state = HookState.Reset.ToString();
            UdpSclient.Instance.SendDataToSever(ParmaterCodes.HookData, hookData);
        });
    }

    public void TurnTableValueChange()
    {
        TurnTableText.text =(int)TurnTableSlider.value + "°";
        TurnTableData Data = new TurnTableData();
        Data.value = (int)TurnTableSlider.value;
        UdpSclient.Instance.SendDataToSever(ParmaterCodes.TurnTableData, Data);
    }

    public void CraneHandValueChange()
    {
        CraneHandText.text = CraneHandSlider.value + "°";
        CraneHandData Data = new CraneHandData();
        Data.value = TurnTableSlider.value;
        UdpSclient.Instance.SendDataToSever(ParmaterCodes.CraneHandData, Data);
    }

    public override void Open()
    {
        base.Open();
        Reset();
    }

    public void Reset()
    {
        //Hide();
        TurnTableSlider.value = 0;
        CraneHandSlider.value = 0;
        TurnTableText.text = "0°";
        CraneHandText.text = "0°";
        HookDown.Reset();
        HookUp.Reset();
    }
}
