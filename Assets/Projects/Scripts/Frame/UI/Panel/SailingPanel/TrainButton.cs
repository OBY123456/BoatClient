using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class TrainButton : MonoBehaviour
{

    public Button button;
    public Image CircleImage, LineImage;
    public Text text;
    public Color color1, color2;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        CircleImage = transform.GetChild(0).GetComponent<Image>();
        LineImage = transform.GetChild(1).GetComponent<Image>();
        text = transform.GetChild(2).GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        CircleImage.color = color1;
        LineImage.color = color1;
        text.color = color1;
    }

    public void Reset()
    {
        CircleImage.color = Color.white;
        LineImage.color = color2;
        text.color = Color.white;
    }
}
