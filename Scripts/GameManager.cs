using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gem;
using Vortex;

public class GameManager : MonoBehaviour
{
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

    [SerializeField] Transform husband;
    [SerializeField] Transform wife;
    [SerializeField] Transform mainCamera;
     FAnimator wifeAnim;
    [SerializeField] AnimationClip wifeBest;
    [SerializeField] AnimationClip wifeGood;
    [SerializeField] AnimationClip wifeBad;
    [SerializeField] AnimationClip husbandVictory;

    [SerializeField] ParticleSystem victoryEff1, victoryEff2;

    [SerializeField] List<Transform> atms;

    public List<int> targetAmounts;
    [SerializeField] List<int> currentAmount = new List<int>();
    [SerializeField] List<PickableStat> husbandCurrentStorelist = new List<PickableStat>();

    CharacterEntity husbandEntity;
    PlayerController husbandScript;

    private void Start()
    {
        husbandEntity = husband.GetComponent<CharacterEntity>();
        husbandCurrentStorelist = husbandEntity.store;
        husbandRunner = husband.GetComponent<RunnerCharacterControl>();
        husbandScript = husband.GetComponent<PlayerController>();
        wifeAnim = wife.GetComponentInChildren<FAnimator>();
        GetCurrentAmount();
        decisionOb = GetComponent<DecisionMaker>();
        fieldVAtStart = Camera.main.fieldOfView;
        levelProgressButton.gameObject.SetActive(false);
    }

    void GetCurrentAmount()
    {
        currentAmount.Clear();
        if(husbandEntity != null)
        {
            if(husbandCurrentStorelist != null)
            {
                int value;
                for (int i = 0; i < husbandCurrentStorelist.Count; i++)
                {
                    husbandEntity.GetCurrentValueFromStore(husbandCurrentStorelist[i].Tag, out value);
                    currentAmount.Add(value);
                }
            }
        }
    }

    /*
     * check the current amount
     * check if target amount is not null and have amounts
     * check if individual item in target amount is not null // not done
     * check if current amount tag is not cash
     * check if the target amount value equals the current amount value
     * current amount list must have one extra member to target amount list
     */
    [SerializeField] Transform levelManager;
    DecisionMaker decisionOb;
    [SerializeField] int decision;
    [SerializeField] bool fieldViewAtEnd = false;
    float fieldVAtStart;

    [SerializeField] Transform levelProgressButton;

    private void Update()
    {
        GetCurrentAmount();
        if (CheckIfTargetAmountCollected())
        {
            Debug.Log("all items collected");
        }

        if(Mathf.Abs(husband.position.z - wife.position.z) < 4)
        {
            if (!decisionMade)
            {
                MakeDecision();
            }

            if (decisionMade)
            {
                husbandRunner.MakeDead();
                if (decision >= 0)
                {
                    PlayVictory(wifeBest);
                    
                }
                else if (decision >= -5)
                {
                    PlayVictory(wifeGood);
                }
                else
                {
                    wifeAnim.Play(wifeBad);
                    Victory2(wifeBad);
                    levelProgressButton.gameObject.SetActive(true);

                }

                if (!fieldViewAtEnd)
                {
                    CameraTask1();
                    mainCamera.GetComponent<GameCamFollow>().isLevelEnd = true;
                }
                
                //LevelManagerV2.LoadNextLevel();
            }

        }
    }

    void CameraTask1()
    {
        Camera.main.fieldOfView += 0.1f;
        if (Camera.main.fieldOfView - fieldVAtStart > 6)
        {
            fieldViewAtEnd = true;
        }
    }

    void PlayVictory(AnimationClip clipForWife)
    {
        wifeAnim.Play(clipForWife);
        Victory2(husbandVictory);
        if (!victoryEff1.isPlaying)
        {
            victoryEff1.Play();
        }
        if (!victoryEff2.isPlaying)
        {
            victoryEff2.Play();
        }

        levelProgressButton.gameObject.SetActive(true);
    }

    public void ButtonActivity()
    {
        LevelManagerV2.LoadNextLevel();
    }

    bool CheckIfTargetAmountCollected()
    {
        if (husbandCurrentStorelist[0].Tag.name == "cash")
        {
            if(currentAmount.Count > targetAmounts.Count)  // target amount do not have cash
            {
                if (targetAmounts != null && targetAmounts.Count > 0)
                {
                    for (int i = 0; i < targetAmounts.Count; i++)
                    {
                        if (targetAmounts[i] != currentAmount[i + 1])
                        {
                            return false;
                        }
                    }
                    //Debug.Log("this level has matched the targets");
                    return true;
                }
                else
                {
                    Debug.LogError("please store the target amounts properly");
                    return false;
                }
            }
            else
            {
                Debug.LogError(" please store all the target amounts");
                return false;
            }
            
        }
        else
        {
            Debug.LogError("please store the cash tag in the first position in store list of husband(CharacterEntity.CS) ");
            return false;
        }
    }

    RunnerCharacterControl husbandRunner;
    [SerializeField] bool decisionMade = false; 
    void MakeDecision()
    {
        decision = decisionOb.CalculateTotalDecision(husband, targetAmounts);
        Debug.Log(decision);
        decisionMade = true;
    }

    void Victory1(AnimationClip clip)
    {
        husbandRunner.MovementSpeedMultiplier = 0;
        husbandScript.PlayVictory(clip);
    }

    void Victory2(AnimationClip clip)
    {
        husbandRunner.MovementSpeedMultiplier = 0;
        husbandScript.PlayVictory(clip, 1);
    }

    void GoToATM()
    {
        if (CheckIfNearATM(out atm))
        {
            if(atm != null)
            {
                /*
                // turn husband toward the atm
                //move husband towards the atm slowly
                Vector3 dir = atm.position - husband.position;
                husband.LookAt(dir);
                //husband must get stop
                CollectFromATM(atm, dir);
                */
            }

        }
    }

    Transform atm;

    bool CheckIfNearATM(out Transform atm)
    {
        if(atms != null && atms.Count > 0)
        {
            for (int i = 0; i < atms.Count; i++)
            {
                if(atms[i] == null) { continue; }
                else
                {
                    if(Mathf.Abs(atms[i].position.x - husband.position.x) < 1.5)
                    {
                        atm = atms[i];
                        return true;
                    }
                }
            }
            atm = null;
            return false;
        }
        else
        {
            throw new System.Exception("some of atm elements are null");
        }
        
    }

    void CollectFromATM(Transform atm, Vector3 direction)
    {
        if (Mathf.Abs(atm.position.x - husband.position.x) > 0.5f)
        {
            husband.transform.forward += direction.normalized * Time.deltaTime;
        }
    }

    void DoSomethingElseOnLevelEnd()
    {
       
    }

}
