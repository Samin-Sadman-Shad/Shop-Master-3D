using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public Image noCashImage;

    private void Start()
    {
        noCashImage.gameObject.SetActive(false);
    }
}
