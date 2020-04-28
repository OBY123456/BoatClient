using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MTFrame;
using UnityEngine.UI;
using System;

public class SailingPanel : BasePanel
{
    public Slider Speedsliders, WaveSlider;

    public Slider IntensitySlider;

    public Text IntensityText;

    public Toggle CameraToggle;
    public Toggle DayNightToggle;

    public Dropdown dropdown;

    public InputField PositionX;
    public InputField PositionY;
    public Button SetingButton,backButton;

    public override void InitFind()
    {
        base.InitFind();
        Speedsliders = FindTool.FindChildComponent<Slider>(transform, "SpeedSlider/Slider");
        WaveSlider = FindTool.FindChildComponent<Slider>(transform, "WaveSlider/Slider");
        IntensitySlider = FindTool.FindChildComponent<Slider>(transform, "Weather/Slider");
        IntensityText = FindTool.FindChildComponent<Text>(transform, "Weather/Text");
        CameraToggle = FindTool.FindChildComponent<Toggle>(transform, "CameraToggle");
        DayNightToggle = FindTool.FindChildComponent<Toggle>(transform, "DayNightToggle");
        dropdown = FindTool.FindChildComponent<Dropdown>(transform, "Weather/Dropdown");
        PositionX = FindTool.FindChildComponent<InputField>(transform, "TargetPosition/InputField");
        PositionY = FindTool.FindChildComponent<InputField>(transform, "TargetPosition/InputField (1)");
        SetingButton = FindTool.FindChildComponent<Button>(transform, "TargetPosition/Button");
        backButton = FindTool.FindChildComponent<Button>(transform, "backButton");
    }

    public override void InitEvent()
    {
        base.InitEvent();
        dropdown.onValueChanged.AddListener((int value) => { WeatherChange(value); });

        backButton.onClick.AddListener(() => {
            UdpSclient.Instance.SceneChange(SceneName.WaitScene, PanelName.WaitPanel);
        });

        SetingButton.onClick.AddListener(() => {
            TargetPosition targetPosition = new TargetPosition();
            int x = int.Parse(PositionX.text);
            int y = int.Parse(PositionY.text);

            if(x > 40000)
            {
                x = 39500;
            }

            if(y > 40000)
            {
                y = 39500;
            }

            if(x == 0 && y == 0)
            {
                return;
            }

            targetPosition.x = x;
            targetPosition.z = y;

            UdpSclient.Instance.SendDataToSever(ParmaterCodes.TargetPosition, targetPosition);
        });
    }

    public void SpeedChange()
    {
        BoatSpeed index = new BoatSpeed();
        index.speed = Speedsliders.value;
        UdpSclient.Instance.SendDataToSever(ParmaterCodes.BoatSpeed, index);
    }

    //public void TimeChange()
    //{
    //    MTFrame.DateTime index = new DateTime();
    //    index.value = sliders[1].value;
    //    UdpSclient.Instance.SendDataToSever(ParmaterCodes.DateTime, index);
    //}

    //public void LightChange()
    //{
    //    OceanLightData index = new OceanLightData();
    //    index.value = sliders[2].value;
    //    UdpSclient.Instance.SendDataToSever(ParmaterCodes.OceanLightData, index);
    //}
    public void DayNightSwitch()
    {
        if(DayNightToggle.isOn)
        {
            DayNightTime time = new DayNightTime();
            time.DayNight = DayNightCycle.night.ToString();
            UdpSclient.Instance.SendDataToSever(ParmaterCodes.DayNightTime, time);
        }
        else
        {
            DayNightTime time = new DayNightTime();
            time.DayNight = DayNightCycle.day.ToString();
            UdpSclient.Instance.SendDataToSever(ParmaterCodes.DayNightTime, time);
        }
    }


    public void WaveChange()
    {
        OceanWaveSize index = new OceanWaveSize();
        index.value = WaveSlider.value;
        UdpSclient.Instance.SendDataToSever(ParmaterCodes.OceanWaveSize, index);
    }

    public void CameraChange()
    {
        if(CameraToggle.isOn)
        {
            CameraState cameraState = new CameraState();
            cameraState.state = CameraSwitch.Open.ToString();
            UdpSclient.Instance.SendDataToSever(ParmaterCodes.CameraState, cameraState);
        }
        else
        {
            CameraState cameraState = new CameraState();
            cameraState.state = CameraSwitch.Close.ToString();
            UdpSclient.Instance.SendDataToSever(ParmaterCodes.CameraState, cameraState);
        }
    }

    public void IntensityChange()
    {
        WeatherIntensity weatherIntensity = new WeatherIntensity();
        weatherIntensity.value = IntensitySlider.value;
        UdpSclient.Instance.SendDataToSever(ParmaterCodes.WeatherIntensity, weatherIntensity);
        IntensityText.text = IntensitySlider.value.ToString();
    }

    private void WeatherChange(int value)
    {
        WeatherMakerPrecipitationType weatherMakerPrecipitationType;
        switch (value)
        {
            case 0:
                weatherMakerPrecipitationType = WeatherMakerPrecipitationType.None;
                break;
            case 1:
                weatherMakerPrecipitationType = WeatherMakerPrecipitationType.Rain;
                break;
            case 2:
                weatherMakerPrecipitationType = WeatherMakerPrecipitationType.Snow;
                break;
            default:
                weatherMakerPrecipitationType = WeatherMakerPrecipitationType.None;
                break;
        }

        WeatherType weatherType = new WeatherType();
        weatherType.weather = weatherMakerPrecipitationType.ToString();
        weatherType.value = IntensitySlider.value;
        UdpSclient.Instance.SendDataToSever(ParmaterCodes.WeatherType, weatherType);
    }

}
