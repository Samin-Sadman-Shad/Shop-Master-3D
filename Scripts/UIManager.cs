using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Gem;

public class UIManager : MonoBehaviour
{
    // please drag and drop the TMPRO assets to this list
    [SerializeField] List<TextMeshProUGUI> topLeftMenu;
    [SerializeField] List<TextMeshProUGUI> topLeftMenuValues;
    [SerializeField] List<Image> UIIcons;
    [SerializeField] List<Image> UIBarImages;
    [SerializeField] List<Image> UIBarBack;
    [SerializeField] Image UIBarCash;

    [SerializeField] TextMeshProUGUI cashValueText;
    [SerializeField] TextMeshProUGUI cashBackText;

    [SerializeField] Transform husband;
    [SerializeField] Transform gameCore;
    CharacterEntity husbandEntity;

    [SerializeField] int rowOffset;
    [SerializeField] int coloumnOffset;

    private void Start()
    {
        husbandEntity = husband.GetComponent<CharacterEntity>();
        currentValueList = new List<int>();
        goalValueList = new List<int>();
        nameList = new List<string>();
        InstantiateTopLeftMenu();
        //StoreNameText();
        //SetNameToText();
        //SetCurrentValues();
        SetGoalValues();
    }

    void InstantiateTopLeftMenu()
    {
        int n = 1; // could be 2 as well for 3 columns
        for (int i = 0; i < topLeftMenu.Count; i++)
        {
            var rowVector = new Vector3(transform.position.x, transform.position.y - i * rowOffset, transform.position.z);
            var colVector = new Vector3(rowVector.x + coloumnOffset, transform.position.y - i * rowOffset, transform.position.z);
            var colVector2 = new Vector3(rowVector.x + coloumnOffset * n, transform.position.y - i * rowOffset - rowOffset, transform.position.z);
            var colVector3 = new Vector3(colVector.x, colVector.y, colVector.z - 10);
            //topLeftMenu[i].transform.position = rowVector;
            UIIcons[i].transform.position = rowVector;
            //topLeftMenuValues[i].transform.position = colVector;
            if(i < topLeftMenu.Count) // -1 if cash bar is included in ui
            {
                //UIBarImages[i].transform.position = colVector3;
                UIBarBack[i].transform.position = colVector;
                topLeftMenuValues[i].transform.position = colVector3;
            }
        }
        //uncomment when cash bar is included in UI
        //var cashVector = new Vector3(transform.position.x + coloumnOffset * n, transform.position.y , transform.position.z);
        //UIBarCash.transform.position = cashVector;
    }

    /*
     * there will be two field
     * one field will get value from player entity's current value
     *     at first the values must be stored in a list
     * another field will get value from game core's target value
     *     target values will also be stored in a list, first one being null for cash
     * what about cash? ---> cash do not have any target value;
     * check 
     */
    List<string> nameList;

    void StoreNameText()
    {
        if(topLeftMenu != null && topLeftMenu.Count > 0)
        {
            for (int i = 0; i < topLeftMenu.Count; i++)
            {
                nameList.Add(topLeftMenu[i].text);
            }
        }
        else
        {
            throw new System.Exception("top left menue has no text");
        }
        
    }

    [SerializeField] List<int> currentValueList;
    [SerializeField] List<int> goalValueList;

    // both cash bar and fruits bar are updated
    void SetCurrentValues()
    {
        var list = husbandEntity.store;
        int valueToAdd;
        currentValueList.Clear();
        if(list != null && list.Count > 0)
        {
            for (int i = 0; i < list.Count; i++)
            {
                husbandEntity.GetCurrentValueFromStore(list[i].Tag, out valueToAdd);
                currentValueList.Add(valueToAdd);
                if(list[i].Tag.name != "cash")
                {
                    var bar = UIBarImages[i-1].GetComponent<UIBar>();
                    bar.SetBarValue(valueToAdd);
                }
                else
                {
                    //uncomment when cash bar is included in UI
                    /*
                    var bar = UIBarCash.GetComponent<UIBar>();
                    bar.SetBarValue(valueToAdd);
                    */
                    if(valueToAdd > 0)
                    {
                        cashValueText.SetText(valueToAdd.ToString());
                    }
                    else
                    {
                        //cashValueText.SetText(" ");
                        //cashBackText.SetText("No Cash");
                    }
                    
                }

            }
        }
    }

    // cashbar do not need goal
    void SetGoalValues()
    {
        var list = gameCore.GetComponent<GameManager>().targetAmounts;
        if(list != null && list.Count > 0)
        {
            for (int i = 0; i < list.Count; i++)
            {
                goalValueList.Add(list[i]);
                var bar = UIBarImages[i].GetComponent<UIBar>();
                bar.SetMaximumValue(list[i]); //only store the field, sprite has not been changed
                topLeftMenuValues[i].SetText(goalValueList[i].ToString());
            }
        }
        
    }

    void SetNameToText()
    {
        if (nameList != null && nameList.Count > 0)
        {
            topLeftMenu[0].SetText(nameList[0]);
            for (int i = 1; i < topLeftMenu.Count; i++)
            {
                topLeftMenu[i].SetText(nameList[i]);
            }
        }
    }

    void SetValuesToText()
    {
        if (currentValueList != null && goalValueList != null)
        {
            topLeftMenuValues[0].SetText(" " + currentValueList[0]);
            for (int i = 1; i < topLeftMenuValues.Count; i++)
            {
                //topLeftMenuValues[i].SetText(" " + currentValueList[i] + "/" + goalValueList[i - 1]);
                //topLeftMenuValues[i].SetText("a");
            }
        }
        else
        {
            throw new System.Exception("any of the list is null");
        }
    }

    void UpdateTopLeftMenu()
    {
        SetCurrentValues();
        //SetValuesToText();
    }

    private void Update()
    {
        UpdateTopLeftMenu();
    }
}
