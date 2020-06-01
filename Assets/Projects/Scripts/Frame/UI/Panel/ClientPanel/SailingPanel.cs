using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MTFrame;
using UnityEngine.UI;
using System;
using DG.Tweening;

public class SailingPanel : BasePanel
{
    public Slider WaveSlider;

    public Button[] WeatherButtons;
    public Button[] DayNightButtons;
    public Button BackButton;
    public Button SwitchButton;
    public Button[] ViewButtons;

    public TrainButton[] TrainButtons;

    public Sprite[] Weather_Sprite_Click, Weather_Sprite_NotClick, DayNight_Sprite_Click, DayNight_Sprite_NotClick;

    public Color blue;

    public RectTransform View_Black_bg;
    public Text[] ViewTextGroup;
    private Vector2 StartPoint = new Vector2(-159,0);
    private Vector2 EndPoint = new Vector2(159,0);

    public Text TiltleText;

    //测试用按钮
    public Button DriveButton, ResetButton;


    public override void InitFind()
    {
        base.InitFind();
        WaveSlider = FindTool.FindChildComponent<Slider>(transform, "WaveButtonGroup/Slider");

        WeatherButtons = FindTool.FindChildNode(transform, "WeatherButtonGroup").GetComponentsInChildren<Button>();
        DayNightButtons = FindTool.FindChildNode(transform, "DayNightButtonGroup").GetComponentsInChildren<Button>();
        BackButton = FindTool.FindChildComponent<Button>(transform, "BackButtonGroup");
        SwitchButton = FindTool.FindChildComponent<Button>(transform, "SwitchButton");
        //DisplayButton = FindTool.FindChildComponent<Button>(transform, "DisPlayButton");
        ViewButtons = FindTool.FindChildNode(transform, "ViewButtonGroup").GetComponentsInChildren<Button>();

        TrainButtons = FindTool.FindChildNode(transform, "TrainModelButtonGroup").GetComponentsInChildren<TrainButton>();

        View_Black_bg = FindTool.FindChildComponent<RectTransform>(transform, "ViewButtonGroup/bg");
        ViewTextGroup = FindTool.FindChildNode(transform, "ViewButtonGroup").GetComponentsInChildren<Text>();

        TiltleText = FindTool.FindChildComponent<Text>(transform, "TiltleText");

        DriveButton = FindTool.FindChildComponent<Button>(transform, "TestButton/Button");
        ResetButton = FindTool.FindChildComponent<Button>(transform, "TestButton/Button (1)");
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

        for (int i = 0; i < TrainButtons.Length; i++)
        {
            InitTrainButtons(TrainButtons[i].button, i);
        }

        ViewButtons[0].onClick.AddListener(() => {
            ViewFirstPerson();
        });

        ViewButtons[1].onClick.AddListener(() => {

            View_Black_bg.DOAnchorPos(EndPoint, 0.1f).OnComplete(()=> {
                ViewTextGroup[0].color = Color.black;
                ViewTextGroup[1].color = blue;
            });

            CameraState state = new CameraState();
            state.state = CameraSwitch.ThirdPerson.ToString();
            UdpSclient.Instance.SendDataToSever(ParmaterCodes.CameraState, state);
        });

        SwitchButton.onClick.AddListener(() =>
        {
            Text text = SwitchButton.gameObject.transform.GetChild(0).GetComponent<Text>();
            if (text.color == Color.white)
            {
                text.color = blue;
            }
            else
            {
                text.color = Color.white;
            }
        });

        //DisplayButton.onClick.AddListener(() => {
        //    Text text = DisplayButton.gameObject.transform.GetChild(0).GetComponent<Text>();
        //    PuGuanCameraData data = new PuGuanCameraData();
        //    if (text.color == Color.white)
        //    {
        //        text.color = blue;
        //        data.state = PuGuanCameraState.Open.ToString();
        //    }
        //    else
        //    {
        //        text.color = Color.white;
        //        data.state = PuGuanCameraState.Hide.ToString();
        //    }
        //    UdpSclient.Instance.SendDataToSever(ParmaterCodes.PuGuanCameraData, data);
        //});

        BackButton.onClick.AddListener(() =>
        {
            UdpSclient.Instance.SceneChange(SceneName.WaitScene, PanelName.WaitPanel);
        });

        DriveButton.onClick.AddListener(() => {
            AutoDriveData autoDriveData = new AutoDriveData();
            autoDriveData.state = AutoDriveEnum.Start.ToString();
            UdpSclient.Instance.SendDataToSever(ParmaterCodes.AutoDriveData, autoDriveData);
        });

        ResetButton.onClick.AddListener(() => {
            AutoDriveData autoDriveData = new AutoDriveData();
            autoDriveData.state = AutoDriveEnum.Wait.ToString();
            UdpSclient.Instance.SendDataToSever(ParmaterCodes.AutoDriveData, autoDriveData);
        });
    }

    public override void Open()
    {
        base.Open();
        Reset();
    }

    private void InitWeatherButtons(Button button , int i)
    {
        button.onClick.AddListener(()=>{

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

    private void InitDayNightButtons(Button button,int i)
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

    private void InitTrainButtons(Button button,int i)
    {
        button.onClick.AddListener(() => {

            RestTrainButtonColor();
            TrainButtons[i].OnClick();

            TiltleText.text = TrainButtons[i].text.text;

            TrainModelData data = new TrainModelData();
            TrainModel trainModel = (TrainModel)Enum.ToObject(typeof(TrainModel), i);
            data.trainModel = trainModel.ToString();
            UdpSclient.Instance.SendDataToSever(ParmaterCodes.TrainModelData, data);
        });
    }

    private void RestTrainButtonColor()
    {
        foreach (TrainButton item in TrainButtons)
        {
            item.Reset();
        }
    }

    private void ViewFirstPerson()
    {
        View_Black_bg.DOAnchorPos(StartPoint, 0.1f).OnComplete(()=> {
            ViewTextGroup[0].color = blue;
            ViewTextGroup[1].color = Color.black;
        });

        CameraState state = new CameraState();
        state.state = CameraSwitch.FirstPerson.ToString();
        UdpSclient.Instance.SendDataToSever(ParmaterCodes.CameraState, state);
    }

    public void WaveChange()
    {
        OceanWaveSize index = new OceanWaveSize();
        index.value = WaveSlider.value;
        UdpSclient.Instance.SendDataToSever(ParmaterCodes.OceanWaveSize, index);
    }

    //public void IntensityChange()
    //{
    //    WeatherIntensity weatherIntensity = new WeatherIntensity();
    //    weatherIntensity.value = IntensitySlider.value;
    //    UdpSclient.Instance.SendDataToSever(ParmaterCodes.WeatherIntensity, weatherIntensity);
    //    IntensityText.text = IntensitySlider.value.ToString();
    //}
    private void Reset()
    {
        ReSetWeatherButtonSprite();
        WeatherButtons[0].gameObject.GetComponent<Image>().sprite = Weather_Sprite_Click[0];

        RestDayNightButtonSprite();
        DayNightButtons[0].gameObject.GetComponent<Image>().sprite = DayNight_Sprite_Click[0];

        WaveSlider.value = 0;

        ViewFirstPerson();

        SwitchButton.gameObject.transform.GetChild(0).GetComponent<Text>().color = Color.white;
        //DisplayButton.gameObject.transform.GetChild(0).GetComponent<Text>().color = Color.white;

        RestTrainButtonColor();
        TrainButtons[0].OnClick();
    }
}
