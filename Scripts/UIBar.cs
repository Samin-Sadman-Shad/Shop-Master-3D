using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Gem;

public class UIBar : MonoBehaviour
{
    [SerializeField] Image imageBar;
    [SerializeField] int max;
    [SerializeField] int min;
    [SerializeField] StatTag tag;

    private void Start()
    {
        imageBar = GetComponent<Image>();
    }

    public float GetBarValue()
    {
        return imageBar.fillAmount;
    }

    public bool SetBarValue(float value)
    {
        if(imageBar != null)
        {
            imageBar.fillAmount = ImageFunction(value);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SetMaximumValue(int value)
    {
        max = value; // only store the field, sprite has not been changed
    }

    public void SetTag(StatTag inputTag)
    {
        tag = inputTag;
    }

    float ImageFunction(float value)
    {
        float result = value / (max - min);
        return result;
    }
}
