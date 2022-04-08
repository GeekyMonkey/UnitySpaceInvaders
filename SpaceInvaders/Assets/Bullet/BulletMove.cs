using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    public float SpeedMin = 80;
    public float SpeedMax = 100;

    public float Life = 3;

    private float Speed;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, Life);
        Speed = GameManger.instance.SpeedFromAlienCount(SpeedMin, SpeedMax);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0, Speed * Time.deltaTime, 0));
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject, 0);
    }
}
