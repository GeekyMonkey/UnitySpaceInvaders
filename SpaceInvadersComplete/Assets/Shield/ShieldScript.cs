using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldScript : MonoBehaviour
{
    public float ExplosionRadiusMin = 3;
    public float ExplosionRadiusMax = 7;
    public float ExplosionForce = 100;
    public float ExplosionSeconds = 5;
    public GameObject ExplosionPrefab;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        // bullet
        // Debug.Log("Shield trigger " + other.gameObject.tag);
        // if (other.gameObject.tag == "Bullet")
        // {
        //     this.ExplodeFrom(other.transform.position);
        //     Destroy(other.gameObject);
        // }
    }

    private void OnCollisionEnter(Collision other)
    {
        // Debug.Log("Shield collide " + other.gameObject.tag);
        // if (other.gameObject.tag == "Missile")
        // {
        //     this.ExplodeFrom(other.transform.position);
        //     Destroy(other.gameObject);
        // }

    }

    public void ExplodeFrom(Vector3 hitPosition)
    {
        var pixels = this.GetComponentsInChildren<PixelScript>(false);
        Vector3 explosionPoint = new Vector3(hitPosition.x, hitPosition.y, transform.Find("ExplosionPoint").position.z);
        foreach (var pixel in pixels)
        {
            float distance = Vector3.Distance(pixel.transform.position, hitPosition);
            bool destroyPixel = distance <= ExplosionRadiusMin;
            if (!destroyPixel && distance <= ExplosionRadiusMax)
            {
                destroyPixel = Random.Range(ExplosionRadiusMin, ExplosionRadiusMax) <= distance;
            }
            if (destroyPixel)
            {
                pixel.ExplodeFrom(explosionPoint, ExplosionForce, ExplosionSeconds);
                // pixel.gameObject.GetComponent<MeshRenderer>().material = BurntPixelMaterial;    
            }
        }

        Instantiate(ExplosionPrefab, explosionPoint, Quaternion.identity);
    }
}