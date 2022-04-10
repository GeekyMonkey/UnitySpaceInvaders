using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    public float Speed = 130;

    public GameObject MissileHitExplosion;

    public float Life = 3;


    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, Life);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0, Speed * Time.deltaTime, 0));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Missile")
        {
            // Debug.Log("Explode at " + other.transform.position.ToString());
            Instantiate(MissileHitExplosion, other.transform.position, Quaternion.identity);
        }
        Destroy(gameObject, 0);
    }
}
