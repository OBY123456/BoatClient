using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderControl : MonoBehaviour
{
    public Slider slider;
    public Text text;
    // Start is called before the first frame update
    void Start()
    {
        text.text = slider.value.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SliderValueChange()
    {
        text.text = slider.value.ToString();
    }
}
