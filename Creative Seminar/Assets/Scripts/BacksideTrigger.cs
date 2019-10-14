using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BacksideTrigger : MonoBehaviour
{
    private void OnTriggerEnter (Collider other)
    {
        other.GetComponent<ComeToMe>().PlaneCross();
    }
}
