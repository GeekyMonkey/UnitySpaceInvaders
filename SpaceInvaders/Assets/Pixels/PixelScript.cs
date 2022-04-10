using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class PixelScript : MonoBehaviour
{
    public void ExplodeFrom(Vector3 explosionPosition, float explosionForce, float explosionSeconds)
    {
        gameObject.layer = 6; // Pixel layer to avoid collisions
        Rigidbody rb = this.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.AddExplosionForce(explosionForce, explosionPosition, 8, 5f, ForceMode.Impulse);

        GameObject.Destroy(this.gameObject, explosionSeconds);
    }

}
