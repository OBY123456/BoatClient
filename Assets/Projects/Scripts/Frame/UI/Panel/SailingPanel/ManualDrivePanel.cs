using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MTFrame;
using UnityEngine.UI;

public class ManualDrivePanel : BasePanel
{
    public Slider SpeedSlider;
    public Text SpeedText;
    public BlueButton[] blueButtons;
    public Button ResetButton;
    public Button AutoDriveButton;
    public ZhuanChangPanel ZhuanchangPanel;

    public override void InitFind()
    {
        base.InitFind();
        SpeedSlider = FindTool.FindChildComponent<Slider>(transform, "BG2/Speed/Slider");
        SpeedText = FindTool.FindChildComponent<Text>(transform, "BG2/Speed/Text (1)");
        blueButtons = FindTool.FindChildNode(transform, "BG2/ControlButtonGroup").GetComponentsInChildren<BlueButton>();
        ResetButton = FindTool.FindChildComponent<Button>(transform, "BG2/Reset");
        AutoDriveButton = FindTool.FindChildComponent<Button>(transform, "BG2/CloseButton");
        ZhuanchangPanel = FindTool.FindParentComponent<ZhuanChangPanel>(transform);
    }

    public override void InitEvent()
    {
        base.InitEvent();
        blueButtons[0].button.onClick.AddListener(() => {

            DriveTurnData turnData = new DriveTurnData();
            if(blueButtons[0].state == BlueButton.State.write)
            {
                turnData.state = DriveTurn.TurnLeft.ToString();
                blueButtons[0].text.color = blueButtons[0].blue;
                blueButtons[0].state = BlueButton.State.blue;
            }
            else
            {
                turnData.state = DriveTurn.Complete.ToString();
                blueButtons[0].text.color = Color.white;
                blueButtons[0].state = BlueButton.State.write;
            }
            ResetBlueButtons(0);
            UdpSclient.Instance.SendDataToSever(ParmaterCodes.AutoDriveData, turnData);
        });

        blueButtons[1].button.onClick.AddListener(() => {

            DriveTurnData turnData = new DriveTurnData();
            if (blueButtons[1].state == BlueButton.State.write)
            {
                turnData.state = DriveTurn.TurnRight.ToString();
                blueButtons[1].text.color = blueButtons[1].blue;
                blueButtons[1].state = BlueButton.State.blue;
            }
            else
            {
                turnData.state = DriveTurn.Complete.ToString();
                blueButtons[1].text.color = Color.white;
                blueButtons[1].state = BlueButton.State.write;
            }
            ResetBlueButtons(1);
            UdpSclient.Instance.SendDataToSever(ParmaterCodes.AutoDriveData, turnData);
        });

        blueButtons[2].button.onClick.AddListener(() => {

            DriveTurnData turnData = new DriveTurnData();
            if (blueButtons[2].state == BlueButton.State.write)
            {
                turnData.state = DriveTurn.TurnBack.ToString();
                blueButtons[2].text.color = blueButtons[2].blue;
                blueButtons[2].state = BlueButton.State.blue;
            }
            else
            {
                turnData.state = DriveTurn.Complete.ToString();
                blueButtons[2].text.color = Color.white;
                blueButtons[2].state = BlueButton.State.write;
            }
            ResetBlueButtons(2);
            UdpSclient.Instance.SendDataToSever(ParmaterCodes.AutoDriveData, turnData);
        });

        ResetButton.onClick.AddListener(() => {
            AutoDriveData autoDriveData = new AutoDriveData();
            autoDriveData.state = AutoDriveSwitch.Reset.ToString();
            UdpSclient.Instance.SendDataToSever(ParmaterCodes.AutoDriveData, autoDriveData);
            foreach (BlueButton item in blueButtons)
            {
                item.Reset();
            }
        });

        AutoDriveButton.onClick.AddListener(() => {
            ZhuanchangPanel.AutoDriveOnclick();
            ZhuanchangPanel.sailingPanel.PuguanPanel.gameObject.SetActive(true);
            Hide();
        });
    }

    private void ResetBlueButtons(int number)
    {
        for (int i = 0; i < blueButtons.Length; i++)
        {
            if(i!= number)
            {
                blueButtons[i].Reset();
            }
        }
    }

    public void SpeedChange()
    {
        SpeedText.text = SpeedSlider.value + "节";
        DriveSpeed driveSpeed = new DriveSpeed();
        driveSpeed.value = (int)SpeedSlider.value;
        UdpSclient.Instance.SendDataToSever(ParmaterCodes.DriveSpeed, driveSpeed);
    }

    private void Reset()
    {
        SpeedText.text = "0节";
        SpeedSlider.value = 0;

        foreach (BlueButton item in blueButtons)
        {
            item.Reset();
        }
    }

    public override void Open()
    {
        base.Open();
        Reset();
    }
}
