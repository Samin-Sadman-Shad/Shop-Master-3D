using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gem;
using UnityEngine.UI;

public class CashCondition : IPickableCondition
{
    [SerializeField] GemEntityTag husbandTag;
    [SerializeField] StatTag cashTag;
    [SerializeField] int lowestCash;
    

    bool IPickableCondition.IsValid()
    {
        int cashNow = 0;
        var husband = GemEntity.GetEntityWithTag<CharacterEntity>(husbandTag);

         husband.GetCurrentValueFromStore(cashTag, out cashNow);
        var noCash = !(cashNow >= lowestCash);
        if (noCash)
        {
            var ui = husband.GetComponent<PlayerUI>().noCashImage;
            ui.gameObject.SetActive(true);
            husband.StartCoroutine(OffUI(ui.gameObject));
        }
        return cashNow >= lowestCash;
    }

    IEnumerator OffUI(GameObject obj)
    {
        yield return new WaitForSeconds(1f);
        obj.SetActive(false);
    }
}
