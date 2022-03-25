using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gem;
using Vortex;

[System.Serializable]
public class CollectCashFromATM : IPickableTask
{
    [SerializeField] AnimationClip cashCollectingClip;
    [SerializeField] AnimationClip runnerClip;
    [SerializeField] ParticleSystem cashCollectingEffect;
    float primaryMult;
    void IPickableTask.OnEquip(IAbsorbable absorber, IPickable pickable)
    {
        //Implement this method so that it will appear on the pickable task list
        //This is invoke when a pickable is equiped by a valid character entity
        //'absorber' is the character entity which absorbes the 'pickable'
        pickable.Entity.StartCoroutine(StopAndCollect(absorber));
    }

    IEnumerator StopAndCollect(IAbsorbable absorber)
    {
        var player = absorber.Entity.GetComponent<PlayerController>();
        var playerRunner = absorber.Entity.GetComponent<RunnerCharacterControl>();
        var playerAnimation = absorber.AbsorberTransform.GetComponent<FAnimator>();

        primaryMult = playerRunner.MovementSpeedMultiplier;
        playerRunner.MovementSpeedMultiplier = 0;
        cashCollectingEffect.Play();
        player.cashUp.gameObject.SetActive(true);
        player.cashUp.Play();

        //playerAnimation.Play(cashCollectingClip);
        yield return new WaitForSeconds(1);
        playerRunner.MovementSpeedMultiplier = primaryMult;
        //playerAnimation.Play(runnerClip);
    }
}