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
        uiAnims[0].SetTrigger("MoveUp");

    }

    public void MovingBackward(int i)
    {
        print("movingDown");
        uiAnims[0].SetTrigger("MoveDown");
    }

    public void FailedMoveUp()
    {
        uiAnims[0].SetTrigger("FailedMoveUp");
    }
    public void FailedMoveDown()
    {
        uiAnims[0].SetTrigger("FailedMoveDown");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
