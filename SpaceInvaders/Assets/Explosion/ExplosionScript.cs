using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 3.0f);
    }

}
