using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlueButton : MonoBehaviour
{
    public Button button;
    public Color blue;
    public Text text;
    public State state = State.write;
    public enum State
    {
        write,
        blue,
    }

    public void Reset()
    {
        state = State.write;
        text.color = Color.white;
    }
}
