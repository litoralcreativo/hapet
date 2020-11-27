using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GridInfoItemScript : MonoBehaviour
{
    public float value;
    public TextMeshProUGUI text;
    public Text _text;

    public void SetText(bool last = false)
    {
        if (!last)
        {
            _text.fontStyle = FontStyle.Normal;
            //text.fontStyle = FontStyles.Normal;
            //text.text = value.ToString("#.###") + " ms";
            _text.text = value.ToString("#.###") + " ms";
        }
        else
        {
            _text.fontStyle = FontStyle.Bold;
            //text.fontStyle = FontStyles.Bold;
            //text.text = "Avr: " + value.ToString("#.###") + " ms";
            _text.text = "Avr: " + value.ToString("#.###") + " ms";
        }
    }

    public void SetSymmetryText()
    {
        _text.fontStyle = FontStyle.Normal;
        //text.fontStyle = FontStyles.Normal;
        //text.text = value.ToString("#.###");
        _text.text = value.ToString("#.###");
    }
}
