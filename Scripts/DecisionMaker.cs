using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gem;

public class DecisionMaker : MonoBehaviour
{
    public bool isAllCollected;
    public int collectionCash;
    int extraCash;

    int totalCollectedStats;
    List<int> collectedStats;

    // 0 for perfect, 1 for good, 2 for bad
    /*
     * check if total collected one is equal to the targeted one
     * if collected ones are of 50-80% of the targeted ones, it is good
     * if below 50% , it is bad
     */
    int CalculateDecisionValue(int collectedProps, int targetProps)
    {
        if(collectedProps >= 0 && targetProps >= 0)
        {
            int decisionExpression = collectedProps - targetProps;
            //Debug.Log("collected " + collectedProps);
            //Debug.Log("target " + targetProps);
            //Debug.Log("decision value " + decisionExpression);

            return decisionExpression;
        }
        else
        {
            throw new System.Exception("collected and target list is not working properly");
        }
        
    }

    /*
     * loop through all the store of player entity
     * add the total of all one by one in the loop
     * the less total is , the better it is
     * 0 for perfect, 1 * stat.count is good, greater than it is bad
     */
    public int CalculateTotalDecision(Transform husband, List<int> targetAmount) 
    {
        int cumulatedResult = 0;
        collectedStats = new List<int>();
        var husbandEntity = husband.GetComponent<CharacterEntity>();
        var store = husbandEntity.store;
        int value;

        for (int i = 1; i < store.Count; i++)
        {
            if(store[i].Tag.name != "cash")
            {
                husbandEntity.GetCurrentValueFromStore(store[i].Tag, out value);
                //Debug.Log(store[i].Tag.ToString() + value);
                collectedStats.Add(value);
            }
        }

        if(targetAmount != null && targetAmount.Count > 0 && collectedStats != null && collectedStats.Count > 0)
        {
            if (collectedStats.Count == targetAmount.Count)
            {
                //Debug.Log("both have the same amount");
                int counter = collectedStats.Count;
                for (int i = 0; i < counter; i++)
                {
                    //Debug.Log(i + " no collected " + collectedStats[i]);
                    //Debug.Log(i + " no target " + targetAmount[i]);
                    var v = CalculateDecisionValue(collectedStats[i], targetAmount[i]);
                    cumulatedResult += v;
                    //Debug.Log(cumulatedResult);
                }
            }
            else
            {
                Debug.LogError("collected types and target types number are not same");
            }
        }
        else
        {
            Debug.LogError("no target amount list has been instantiated/assigned in the Game Core");
        }
        
        return cumulatedResult;
    }

}
