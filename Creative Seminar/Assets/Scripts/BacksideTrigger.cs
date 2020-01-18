using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BacksideTrigger : MonoBehaviour
{
    private void OnTriggerExit (Collider other)
    {
        if (other.GetComponent<ComeToMe>().discovered)
        {
            other.GetComponent<ComeToMe>().PlaneCross();
        }
    }
}
