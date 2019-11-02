using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BacksideTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        print("Hit");
        other.GetComponent<ComeToMe>().PlaneCross(true);
    }
    private void OnTriggerExit (Collider other)
    {
        other.GetComponent<ComeToMe>().PlaneCross(false);
    }
}
