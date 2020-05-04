using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public Animator[] uiAnims;
    List<string> upStates = new List<string>()
    {
        "0900Selected","0900ShrinkUp", "0900UpAway", "0900UpAway"

    };
    List<string> downStates = new List<string>()
    {
        "0900Selected","0900ShrinkDown", "0900DownAway","0900Away"
    };

    private void Start()
    {
    }

    public void MovingForward(int timeFrame)
    {
        for (int i = 0; i < uiAnims.Length; i++)
        {
            if(uiAnims[i] != null)
            {
                if (!uiAnims[i].GetBool("OffScreenDown"))
                {
                    uiAnims[0].SetTrigger("MoveUp");
                    if (uiAnims[i].GetCurrentAnimatorStateInfo(0).IsName("0900UpAway"))
                    {
                        uiAnims[i].SetInteger("OffScreenNum", 1);
                        uiAnims[i].SetBool("OffScreenUp", true);
                    }
                }
                else
                {
                    int j = uiAnims[i].GetInteger("OffScreenNum");
                    uiAnims[i].SetInteger("OffScreenNum", j + 1);
                    print(uiAnims[i].GetInteger("OffScreenNum"));
                }
                OffScreenHandler(uiAnims[i], "OffScreenDown", "MoveUp");
            }
        }

    }
    public void AddingTimeframe(int addIndex, Animator newAnim, int currentTime)
    {
        int diff = Mathf.Abs(currentTime - addIndex);
        uiAnims[addIndex] = newAnim;
        if(currentTime > addIndex)
        {
            for(int m = addIndex; m < diff; m++)
            {
                if(uiAnims[m] == null)
                {
                    diff = m;
                }
            }
            Debug.Log(diff);
            uiAnims[addIndex].Play(upStates[diff]);
        }
        else if(currentTime < addIndex)
        {
            for (int m = addIndex; m > 0; m--)
            {
                if(uiAnims[m] == null)
                {
                    diff = m;
                }
            }
            Debug.Log(diff);
            uiAnims[addIndex].Play(upStates[diff]);
        }else if(currentTime == addIndex)
        {
            uiAnims[addIndex].Play(upStates[0]);
        }
    }
    public void MovingBackward(int timeFrame)
    {
        for (int i = 0; i < uiAnims.Length; i++)
        {
            if (uiAnims[i] != null)
            {
                if (!uiAnims[0].GetBool("OffScreenDown"))
                {
                    print("movingDown");
                    uiAnims[i].SetTrigger("MoveDown");
                    if (uiAnims[i].GetCurrentAnimatorStateInfo(0).IsName("0900DownAway"))
                    {
                        uiAnims[i].SetInteger("OffScreenNum", 1);
                        uiAnims[i].SetBool("OffScreenDown", true);
                    }
                }
                else
                {
                    int j = uiAnims[i].GetInteger("OffScreenNum");
                    uiAnims[i].SetInteger("OffScreenNum", j + 1);
                    print(uiAnims[i].GetInteger("OffScreenNum"));
                }
                OffScreenHandler(uiAnims[i], "OffScreenUp", "MoveDown");
            }
        }
                

    }

    public void FailedMoveUp()
    {
        for (int i = 0; i < uiAnims.Length; i++)
        {
            if (uiAnims[i] != null)
            {
                uiAnims[i].SetTrigger("FailedMoveUp");
            }
        }
                
    }
    public void FailedMoveDown()
    {
        for (int i = 0; i < uiAnims.Length; i++)
        {
            if (uiAnims[i] != null)
            {
                uiAnims[i].SetTrigger("FailedMoveDown");
            }
        }
    }
    public void OffScreenHandler(Animator anim, string Direction, string Trigger)
    {
        if (anim.GetBool(Direction))
        {
            int k = anim.GetInteger("OffScreenNum");
            if (k > 0)
            {
                anim.SetInteger("OffScreenNum", k-1);
            }
            if (anim.GetInteger("OffScreenNum") == 0)
            {
                anim.SetBool(Direction, false);
                anim.SetTrigger(Trigger);
            }
        }
    }
}
