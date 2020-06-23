using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MTFrame;
using UnityEngine.UI;
using System;

public class ZhuanChangPanel : BasePanel
{
    public Slider WaveSlider;

    public Button[] WeatherButtons;
    public Button[] DayNightButtons;
    public Button ResetButton;
    //public Button SwitchButton, DisplayButton;
    public BlueButton SwitchButton, DisplayButton;
    public BlueButton AutoDrive, ManualDrive;

    public Sprite[] Weather_Sprite_Click, Weather_Sprite_NotClick, DayNight_Sprite_Click, DayNight_Sprite_NotClick;

    public Color blue;

    public SliderButton ViewSwitch, ModelSwitch;

    public ManualDrivePanel ManualdrivePanel;
    public SailingPanel sailingPanel;

    public CanvasGroup mask;

    public override void InitFind()
    {
        base.InitFind();
        WaveSlider = FindTool.FindChildComponent<Slider>(transform, "WaveButtonGroup/Slider");

        WeatherButtons = FindTool.FindChildNode(transform, "WeatherButtonGroup/Button").GetComponentsInChildren<Button>();
        DayNightButtons = FindTool.FindChildNode(transform, "DayNightButtonGroup/Button").GetComponentsInChildren<Button>();

        SwitchButton = FindTool.FindChildComponent<BlueButton>(transform, "SwitchButton");
        DisplayButton = FindTool.FindChildComponent<BlueButton>(transform, "DisPlayButton");
        AutoDrive = FindTool.FindChildComponent<BlueButton>(transform, "DriveButton/AutoButton");
        ManualDrive = FindTool.FindChildComponent<BlueButton>(transform, "DriveButton/ManualButton");

        ViewSwitch = FindTool.FindChildComponent<SliderButton>(transform, "ViewButtonGroup");
        ModelSwitch = FindTool.FindChildComponent<SliderButton>(transform, "ViewButtonGroup (1)");

        ResetButton = FindTool.FindChildComponent<Button>(transform, "DriveButton/ResetButton");

        ManualdrivePanel = FindTool.FindChildComponent<ManualDrivePanel>(transform, "ManualDrivePanel");

        mask = FindTool.FindChildComponent<CanvasGroup>(transform, "mask");

        sailingPanel = FindTool.FindParentComponent<SailingPanel>(transform);
    }

    public override void InitEvent()
    {
        base.InitEvent();

        for (int i = 0; i < WeatherButtons.Length; i++)
        {
            InitWeatherButtons(WeatherButtons[i], i);
        }

        for (int i = 0; i < DayNightButtons.Length; i++)
        {
            InitDayNightButtons(DayNightButtons[i], i);
        }

        ViewSwitch.ButtonLeft.onClick.AddListener(() => {
            if (ViewSwitch.ButtonState == SliderButton.State.Right)
            {
                CameraState state = new CameraState();
                state.state = CameraSwitch.FirstPerson.ToString();
                UdpSclient.Instance.SendDataToSever(ParmaterCodes.CameraState, state);
                ViewSwitch.ButtonState = SliderButton.State.Left;
            }
            ModelSwitch.Reset();
            ModelSwitch.gameObject.SetActive(true);
        });

        ViewSwitch.ButtonRight.onClick.AddListener(() => {
            if (ViewSwitch.ButtonState == SliderButton.State.Left)
            {
                CameraState state = new CameraState();
                state.state = CameraSwitch.ThirdPerson.ToString();
                UdpSclient.Instance.SendDataToSever(ParmaterCodes.CameraState, state);
                ViewSwitch.ButtonState = SliderButton.State.Right;
            }
            ModelSwitch.gameObject.SetActive(false);
        });

        ModelSwitch.ButtonLeft.onClick.AddListener(() => {
            if (ModelSwitch.ButtonState == SliderButton.State.Right)
            {
                CameraState state = new CameraState();
                state.state = CameraSwitch.FirstPerson.ToString();
                UdpSclient.Instance.SendDataToSever(ParmaterCodes.CameraState, state);
                ModelSwitch.ButtonState = SliderButton.State.Left;
            }

        });

        ModelSwitch.ButtonRight.onClick.AddListener(() => {
            if (ModelSwitch.ButtonState == SliderButton.State.Left)
            {
                CameraState state = new CameraState();
                state.state = CameraSwitch.RearView.ToString();
                UdpSclient.Instance.SendDataToSever(ParmaterCodes.CameraState, state);
                ModelSwitch.ButtonState = SliderButton.State.Right;
            }

        });

        SwitchButton.button.onClick.AddListener(() =>
        {
            if (SwitchButton.state == BlueButton.State.blue)
            {
                SwitchButton.text.color = Color.white;
                SwitchButton.state = BlueButton.State.write;
            }
            else
            {
                SwitchButton.text.color = SwitchButton.blue;
                SwitchButton.state = BlueButton.State.blue;
            }
        });

        DisplayButton.button.onClick.AddListener(() =>
        {
            PuGuanCameraData data = new PuGuanCameraData();
            if (DisplayButton.state == BlueButton.State.write)
            {
                DisplayButton.text.color = blue;
                DisplayButton.state = BlueButton.State.blue;
                data.state = PuGuanCameraState.Open.ToString();
            }
            else
            {
                DisplayButton.text.color = Color.white;
                DisplayButton.state = BlueButton.State.write;
                data.state = PuGuanCameraState.Hide.ToString();
            }
            UdpSclient.Instance.SendDataToSever(ParmaterCodes.PuGuanCameraData, data);
        });

        AutoDrive.button.onClick.AddListener(() =>
        {
            AutoDriveOnclick();
        });

        //DriveButton.onClick.AddListener(() => {
        //    AutoDriveData autoDriveData = new AutoDriveData();
        //    autoDriveData.state = AutoDriveEnum.Start.ToString();
        //    UdpSclient.Instance.SendDataToSever(ParmaterCodes.AutoDriveData, autoDriveData);
        //});
        ManualDrive.button.onClick.AddListener(() =>
        {
            sailingPanel.PuguanPanel.gameObject.SetActive(false);
            if (ManualDrive.state == BlueButton.State.write)
            {
                
                AutoDriveData autoDriveData = new AutoDriveData();
                ManualDrive.text.color = blue;
                ManualDrive.state = BlueButton.State.blue;
                autoDriveData.state = AutoDriveSwitch.Close.ToString();
                UdpSclient.Instance.SendDataToSever(ParmaterCodes.AutoDriveData, autoDriveData);

                ManualdrivePanel.Open();
            }

            if (AutoDrive.state == BlueButton.State.blue)
            {
                AutoDrive.text.color = Color.white;
                AutoDrive.state = BlueButton.State.write;
            }
        });

        ResetButton.onClick.AddListener(() => {
            AutoDriveData autoDriveData = new AutoDriveData();
            autoDriveData.state = AutoDriveSwitch.Reset.ToString();
            UdpSclient.Instance.SendDataToSever(ParmaterCodes.AutoDriveData, autoDriveData);

            AutoDrive.Reset();
            ManualDrive.Reset();

        });
    }

    public override void Open()
    {
        base.Open();
        MaskHide();
    }

    public void AutoDriveOnclick()
    {
        if (AutoDrive.state == BlueButton.State.write)
        {
            AutoDriveData autoDriveData = new AutoDriveData();
            AutoDrive.text.color = blue;
            AutoDrive.state = BlueButton.State.blue;
            autoDriveData.state = AutoDriveSwitch.Open.ToString();
            UdpSclient.Instance.SendDataToSever(ParmaterCodes.AutoDriveData, autoDriveData);

            if (ManualDrive.state == BlueButton.State.blue)
            {
                ManualDrive.text.color = Color.white;
                ManualDrive.state = BlueButton.State.write;
            }
        }
    }

    private void InitWeatherButtons(Button button, int i)
    {
        button.onClick.AddListener(() => {

            ReSetWeatherButtonSprite();
            WeatherButtons[i].gameObject.GetComponent<Image>().sprite = Weather_Sprite_Click[i];

            WeatherType weatherType = new WeatherType();
            WeatherMakerPrecipitationType type = (WeatherMakerPrecipitationType)Enum.ToObject(typeof(WeatherMakerPrecipitationType), i);
            weatherType.weather = type.ToString();
            weatherType.value = 1.0f;
            UdpSclient.Instance.SendDataToSever(ParmaterCodes.WeatherType, weatherType);
        });
    }

    private void ReSetWeatherButtonSprite()
    {
        for (int i = 0; i < WeatherButtons.Length; i++)
        {
            WeatherButtons[i].gameObject.GetComponent<Image>().sprite = Weather_Sprite_NotClick[i];
        }
    }

    private void InitDayNightButtons(Button button, int i)
    {
        button.onClick.AddListener(() => {

            RestDayNightButtonSprite();
            DayNightButtons[i].gameObject.GetComponent<Image>().sprite = DayNight_Sprite_Click[i];

            DayNightTime time = new DayNightTime();
            DayNightCycle dayNight = (DayNightCycle)Enum.ToObject(typeof(DayNightCycle), i);
            time.DayNight = dayNight.ToString();
            UdpSclient.Instance.SendDataToSever(ParmaterCodes.DayNightTime, time);
        });
    }

    private void RestDayNightButtonSprite()
    {
        for (int i = 0; i < DayNightButtons.Length; i++)
        {
            DayNightButtons[i].gameObject.GetComponent<Image>().sprite = DayNight_Sprite_NotClick[i];
        }
    }

    public void MaskOpen()
    {
        mask.alpha = 1;
        mask.blocksRaycasts = true;
    }

    public void MaskHide()
    {
        mask.alpha = 0;
        mask.blocksRaycasts = false;
    }

    public void Reset()
    {
        ReSetWeatherButtonSprite();
        WeatherButtons[0].gameObject.GetComponent<Image>().sprite = Weather_Sprite_Click[0];

        RestDayNightButtonSprite();
        DayNightButtons[0].gameObject.GetComponent<Image>().sprite = DayNight_Sprite_Click[0];

        WaveSlider.value = 0;

        ViewSwitch.Reset();
        ModelSwitch.Reset();
        ModelSwitch.gameObject.SetActive(true);

        SwitchButton.Reset();
        DisplayButton.Reset();
        AutoDrive.Reset();

        //MaskHide();
    }
}
