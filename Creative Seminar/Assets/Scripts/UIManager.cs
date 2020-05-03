using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public Animator[] uiAnims;

    private void Start()
    {
        
    }

    public void MovingForward(int i)
    {
        if (!uiAnims[0].GetBool("OffScreenDown"))
        {
            uiAnims[0].SetTrigger("MoveUp");
            if (uiAnims[0].GetCurrentAnimatorStateInfo(0).IsName("0900UpAway"))
            {
                uiAnims[0].SetInteger("OffScreenNum", 1);
                uiAnims[0].SetBool("OffScreenUp", true);
            }
        }
        else
        {
            int j = uiAnims[0].GetInteger("OffScreenNum");
            uiAnims[0].SetInteger("OffScreenNum", j+1);
            print(uiAnims[0].GetInteger("OffScreenNum"));
        }
        OffScreenHandler(uiAnims[0], "OffScreenDown", "MoveUp");
    }

    public void MovingBackward(int i)
    {
        if (!uiAnims[0].GetBool("OffScreenDown"))
        {
            print("movingDown");
            uiAnims[0].SetTrigger("MoveDown");
            if (uiAnims[0].GetCurrentAnimatorStateInfo(0).IsName("0900DownAway"))
            {
                uiAnims[0].SetInteger("OffScreenNum", 1);
                uiAnims[0].SetBool("OffSceenDown", true);
            }
        }
        else
        {
            int j = uiAnims[0].GetInteger("OffScreenNum");
            uiAnims[0].SetInteger("OffScreenNum", j+1);
            print(uiAnims[0].GetInteger("OffScreenNum"));
        }
        OffScreenHandler(uiAnims[0],"OffScreenUp", "MoveDown");

    }

    public void FailedMoveUp()
    {
        uiAnims[0].SetTrigger("FailedMoveUp");
    }
    public void FailedMoveDown()
    {
        uiAnims[0].SetTrigger("FailedMoveDown");

    }
    public void OffScreenHandler(Animator anim, string Direction, string Trigger)
    {
        if (anim.GetBool(Direction))
        {
            int k = anim.GetInteger("OffScreenNum");
            if (k > 0)
            {
                uiAnims[0].SetInteger("OffScreenNum", k-1);
            }
            if (anim.GetInteger("OffScreenNum") == 0)
            {
                uiAnims[0].SetBool(Direction, false);
                uiAnims[0].SetTrigger(Trigger);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
