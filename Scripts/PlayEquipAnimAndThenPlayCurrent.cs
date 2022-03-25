using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gem;
using Vortex;

[System.Serializable]
public class PlayEquipAnimOnAbsorableAndThenPlayCurrent : IPickableTask
{
    [SerializeField] AnimationClip clipPick;
    [SerializeField] AnimationClip clipRun;
    [SerializeField] float transitionAmount = 0.1f;
    [SerializeField] float speed = 1.0f;

    FAnimator anim;
    FAnimationState currentState;
    void IPickableTask.OnEquip(IAbsorbable absorber, IPickable pickable)
    {
        //Implement this method so that it will appear on the pickable task list
        //This is invoke when a pickable is equiped by a valid character entity
        //'absorber' is the character entity which absorbes the 'pickable'
        
        anim = absorber.AbsorberTransform.GetComponentInChildren<FAnimator>();
        anim.AbortSequenceIfAny();
        currentState = anim.CurrentState;
        absorber.Entity.StartCoroutine(PlayAnim());
    }

    IEnumerator PlayAnim()
    {
        bool completed = false;
        anim.Play(clipPick, transitionAmount, () =>
        {
            completed = true;
            
        }, false, speed);

        while (completed == false)
        {
            yield return null;
        }

        anim.Play(clipRun, transitionAmount, null, true, 1f);
    }
}