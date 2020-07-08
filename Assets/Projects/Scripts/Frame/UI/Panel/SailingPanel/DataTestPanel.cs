using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MTFrame;
using UnityEngine.UI;
using Newtonsoft.Json;

public class DataTestPanel : BasePanel
{
    public Button TimeButton, TemperatureButton, DepthButton, RateButton, LogButton,
        RAPButton, GyroButton, RollButton, WindButton_FirstPerson;

    public Button WindButton_RearView, TrueButton, RelativeButton, RotButton,backButton;

    private DataProtocol dataProtocol = new DataProtocol();

    public override void InitEvent()
    {
        base.InitEvent();
        TimeButton.onClick.AddListener(() => {
            TimeData timeData = new TimeData();
            timeData.Hours = Random.Range(0, 23);
            timeData.Minutes = Random.Range(0, 59);
            timeData.Seconds = Random.Range(0, 59);
            SentData("TimeData", timeData);
        });

        TemperatureButton.onClick.AddListener(() => {
            TemperatureData Data = new TemperatureData();
            Data.value = Random.Range(10.0f, 30.0f);
            SentData("TemperatureData", Data);
        });

        DepthButton.onClick.AddListener(() => {
            DepthData Data = new DepthData();
            Data.value = Random.Range(0f, 200f);
            SentData("DepthData", Data);
        });

        RateButton.onClick.AddListener(() => {
            RoteOfTurnData Data = new RoteOfTurnData();
            Data.value = Random.Range(-30.0f, 30f);
            SentData("RoteOfTurnData", Data);
        });

        LogButton.onClick.AddListener(() => {
            LogData Data = new LogData();
            Data.value1 = Random.Range(0.0f, 9.9f);
            Data.value2 = Random.Range(0.0f, 9.9f);
            Data.value3 = Random.Range(0.0f, 9.9f);
            SentData("LogData", Data);
        });

        RAPButton.onClick.AddListener(() => {
            RAPData Data = new RAPData();
            Data.RPMValue = Random.Range(0.0f, 110f);
            Data.PitchValue = Random.Range(0.0f, 110f);
            Data.AngleValue = Random.Range(-180.0f, 180f);
            Data.direction = ((Direction)System.Enum.ToObject(typeof(Direction), Random.Range(1, 4))).ToString();
            SentData("RAPData", Data);
        });

        GyroButton.onClick.AddListener(() => {
            GyroData Data = new GyroData();
            Data.value = Random.Range(0.0f, 360f);
            SentData("GyroData", Data);
        });

        RollButton.onClick.AddListener(() => {
            RollData Data = new RollData();
            Data.value = Random.Range(-20.0f, 20.0f);
            SentData("RollData", Data);
        });

        WindButton_FirstPerson.onClick.AddListener(() => {
            RelDirectionData Data = new RelDirectionData();
            Data.value = Random.Range(-180.0f, 180.0f);
            SentData("RelDirectionData", Data);

            RelSpeedData Data2 = new RelSpeedData();
            Data2.value = Random.Range(0.0f, 20.0f);
            SentData("RelSpeedData", Data2);
        });

        WindButton_RearView.onClick.AddListener(() => {
            WindData Data = new WindData();
            Data.value = Random.Range(-180.0f, 180.0f);
            SentData("WindData", Data);
        });

        TrueButton.onClick.AddListener(() => {
            TrueData Data = new TrueData();
            Data.value1 = Random.Range(0, 180);
            Data.value2 = Random.Range(0, 50);
            SentData("TrueData", Data);
        });

        RelativeButton.onClick.AddListener(() => {
            RelativeData Data = new RelativeData();
            Data.value1 = Random.Range(0, 180);
            Data.value2 = Random.Range(0, 50);
            SentData("RelativeData", Data);
        });

        RotButton.onClick.AddListener(() => {
            RotData Data = new RotData();
            Data.Rotvalue = Random.Range(-180, 180);
            Data.Maonvalue = Random.Range(0.0f, 50.0f);
            Data.Gyrovalue = Random.Range(0.0f, 50.0f);
            Data.Cobvalue = Random.Range(0.0f, 50.0f);
            Data.Bogvalue = Random.Range(0.0f, 50.0f);
            Data.Pitchvalue = Random.Range(-10.0f, 10.0f);
            SentData("RotData", Data);
        });

        backButton.onClick.AddListener(() => {
            Hide();
        });
    }

    private void SentData(string DataName,object obj )
    {
        dataProtocol.DataMsg = JsonConvert.SerializeObject(obj);
        dataProtocol.DataType = DataName;
        UDPSend.Instance.Send(JsonConvert.SerializeObject(dataProtocol));
    }
}
