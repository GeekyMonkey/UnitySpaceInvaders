using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionLightScript : MonoBehaviour
{                                        
    public float LightLifeSeconds = 0.2f;
    
    private Light PointLight;
    private float Intensity;

    void Start()
    {                                         
        PointLight = GetComponent<Light>();
        Intensity = PointLight.intensity;
        Destroy(gameObject, LightLifeSeconds);
    }                                    

    void Update() {
         PointLight.intensity -= (Intensity / LightLifeSeconds) * Time.deltaTime;
    }

}
