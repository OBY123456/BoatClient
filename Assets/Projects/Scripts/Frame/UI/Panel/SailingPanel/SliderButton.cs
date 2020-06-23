using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SliderButton : MonoBehaviour
{
    public Button ButtonLeft, ButtonRight;
    public Color blue;
    public RectTransform View_Black_bg;
    private Vector2 StartPoint = new Vector2(-159, 0);
    private Vector2 EndPoint = new Vector2(159, 0);
    public Text TextLeft, TextRight;
    public State ButtonState = State.Left;
    public enum State
    {
        Left,
        Right,
    }

    private void Start()
    {

        ButtonLeft.onClick.AddListener(() => {
            View_Black_bg.DOAnchorPos(StartPoint, 0.1f).OnComplete(() => {
                TextLeft.color = blue;
                TextRight.color = Color.black;
            });
        });

        ButtonRight.onClick.AddListener(() => {
            View_Black_bg.DOAnchorPos(EndPoint, 0.1f).OnComplete(() => {
                TextLeft.color = Color.black;
                TextRight.color = blue;
            });
        });
    }

    public void Reset()
    {
        TextLeft.color = blue;
        TextRight.color = Color.black;
        View_Black_bg.anchoredPosition = StartPoint;
        ButtonState = State.Left;
    }

}
