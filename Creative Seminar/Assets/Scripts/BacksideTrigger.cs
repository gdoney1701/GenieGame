using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BacksideTrigger : MonoBehaviour
{
    private void OnTriggerExit (Collider other)
    {
        other.GetComponent<ComeToMe>().PlaneCross();
    }
}
