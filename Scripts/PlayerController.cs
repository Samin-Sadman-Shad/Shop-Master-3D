using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gem;
using Vortex;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Transform lastCylider;
    [SerializeField] Transform cylU;
    [SerializeField] Transform kitchen;
    [SerializeField] Transform kitchenU;
    [SerializeField] Transform character;
    [SerializeField] Transform wife;

    public Transform noCashObject;
     /*
     * husband will follow drag command to move left or right
     * husband can have some cash
     * if cash is greater than zero  
     *       husband can interact with trigger pickables, if the pickables max level is not reached
     *       everytime husband interacts with pickables, cash reduced
     * else
     *       husband may fell down
     *       game over
     *       
     * if atm booth is encountered, husband gets cash
     */

    [SerializeField] StatTag cashTag;
    [SerializeField] int cash;

    [SerializeField] float finalRotateSpeed = 10F;

    [SerializeField] FAnimationClip currentClip;
    public ParticleSystem cashUp;

    CharacterEntity charEntity;
    RunnerCharacterControl runnerEntity;
    FAnimator anim;
    [SerializeField] AnimationClip victoryClip;

    void Start()
    {
        charEntity = GetComponent<CharacterEntity>();
        charEntity.GetCurrentValueFromStore(cashTag, out cash);
        runnerEntity = GetComponent<RunnerCharacterControl>();
        anim = GetComponentInChildren<FAnimator>();
    }

    private void Update()
    {
        currentClip = GetComponentInChildren<FAnimator>().CurrentState.Clip;
    }

    public void PlayVictory(AnimationClip clip)
    {
        Quaternion primaryRotation = character.localRotation;
        Quaternion goal = Quaternion.Euler(0, 180, 0);
        //Debug.Log(Mathf.Abs(character.localRotation.y - 180));
        if(Mathf.Abs(character.localRotation.y - 180) < 181)
        {
            character.localRotation = Quaternion.Slerp(primaryRotation, goal, Time.deltaTime * finalRotateSpeed);
            //Debug.Log("rotating");
        }
        else
        {
            anim.Play(clip);
        }
        //anim.Play(victoryClip);
    }

    public void PlayVictory(AnimationClip clip, int overload)
    {
        Quaternion primaryRotation = character.localRotation;
        character.LookAt(wife);
        
        anim.Play(clip);
    }

    public void PlayLose()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform == lastCylider)
        {
            transform.position = new Vector3(transform.position.x,transform.position.y - (cylU.position.y - transform.position.y), transform.position.z);
            runnerEntity.MovementSpeedMultiplier *= 0.5f;
            //Debug.Log("collided");
        }
        else if(collision.transform == kitchen)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - (kitchenU.position.y - transform.position.y), transform.position.z);
            runnerEntity.MovementSpeedMultiplier *= 0.7f;
            Debug.Log("collided");
        }
    }

}
