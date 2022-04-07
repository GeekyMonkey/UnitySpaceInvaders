using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionLightScript : MonoBehaviour
{                                        
    public float LightLifeSeconds = 0.2f;

    void Start()
    {
        Destroy(gameObject, LightLifeSeconds);
    }

}
