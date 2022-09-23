using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UfoMoveScript : MonoBehaviour
{        
    private float InitialX;             
    private float FinalX;  
    public float Speed = 25;
                           
    // Start is called before the first frame update
    void Start()
    {
        InitialX = transform.position.x;
        FinalX = -InitialX;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Speed * Time.deltaTime, 0, 0);
        if (transform.position.x > FinalX) {
            Destroy(gameObject);
        }        
    }
}
