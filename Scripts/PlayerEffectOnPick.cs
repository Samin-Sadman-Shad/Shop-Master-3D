using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gem;

[System.Serializable]
public class PlayerEffectOnPick : IPickableTask
{
    void IPickableTask.OnEquip(IAbsorbable absorber, IPickable pickable)
    {
        //Implement this method so that it will appear on the pickable task list
        //This is invoke when a pickable is equiped by a valid character entity
        //'absorber' is the character entity which absorbes the 'pickable'
        //todo
    }
}