using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NegativeLight : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Light thisLight = gameObject.GetComponent<Light>();
        thisLight.color = new Color(-1f, -1f, -1f);
    }

}
