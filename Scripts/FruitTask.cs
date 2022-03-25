using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gem;

[System.Serializable]
public class FruitTask : IPickableTask
{
    [SerializeField] int increment;
    void IPickableTask.OnEquip(IAbsorbable absorber, IPickable pickable)
    {
        //Implement this method so that it will appear on the pickable task list
        //This is invoke when a pickable is equiped by a valid character entity
        //'absorber' is the character entity which absorbes the 'pickable'
        pickable.Entity.gameObject.SetActive(false);
        Debug.Log(pickable.Entity.gameObject.name + " is equipped");
    }
}