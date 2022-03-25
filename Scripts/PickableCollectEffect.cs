using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gem;

[System.Serializable]
public class PickableCollectEffect : IPickableTask
{
    [SerializeField] ParticleSystem effects;
    [SerializeField] GemEntityTag positive;
    [SerializeField] GemEntityTag negative;
    void IPickableTask.OnEquip(IAbsorbable absorber, IPickable pickable)
    {
        //Implement this method so that it will appear on the pickable task list
        //This is invoke when a pickable is equiped by a valid character entity
        //'absorber' is the character entity which absorbes the 'pickable'
        effects.Play();
        //effects.gameObject.SetActive(true);
        //effects.transform.parent = absorber.Entity.transform;
        effects.transform.localPosition = new Vector3(0, 1, 0);
        if (pickable.Entity.IsOfType(positive))
        {
            SetColor("#41F800");
        }
        else
        {
            SetColor("#F81500");
        }

    }

    void SetColor(string hexa)
    {
        Color newColor;
        ColorUtility.TryParseHtmlString(hexa, out newColor);
        var main = effects.main;
        main.startColor = newColor;
    }
}