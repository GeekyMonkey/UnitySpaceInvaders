using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;

public class AlienAnimation : MonoBehaviour
{
    public float ExplosionForce = 100f;
    public float ExplosionSeconds = 6f;
    public GameObject ExplosionPrefab;
    public GameObject[] MissilePrefabs;

    private Transform[] AnimationFrames;
    private int FrameCount;
    private int FrameShown = -1;
    private bool Dead = false;


    // Start is called before the first frame update
    void Start()
    {
        FindAnimationFrames();
        ShowFrame(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Dead == false)
        {
            ShowFrame(GameManger.instance.AlienAnimationFrame % FrameCount);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if (other.tag == "Bullet")
        {
            Die(other.transform.position);
        }
    }

    void ShowFrame(int frame)
    {
        if (FrameShown != frame)
        {
            FrameShown = frame;
            for (int i = 0; i < FrameCount; i++)
            {
                bool isFrameVisible = i == frame;
                AnimationFrames[i].gameObject.SetActive(isFrameVisible);
            }
        }
    }

    void FindAnimationFrames()
    {
        AnimationFrames = this.GetComponentsInChildren<Transform>(true)
            .Where(t => t.gameObject.tag == "AnimationFrame").ToArray();
        FrameCount = AnimationFrames.Length;
    }

    void Die(Vector3 bulletPosition)
    {
        if (!Dead)
        {
            Dead = true;
            GameManger.instance.AlienDied(this);

            var pixels = this.GetComponentsInChildren<PixelScript>(false);
            Vector3 explosionPoint = new Vector3(bulletPosition.x, bulletPosition.y, transform.Find("ExplosionPoint").position.z);
            foreach (var pixel in pixels)
            {
                pixel.ExplodeFrom(explosionPoint, ExplosionForce, ExplosionSeconds);
            }

            Instantiate(ExplosionPrefab, explosionPoint, Quaternion.identity);
            GetComponent<BoxCollider>().enabled = false;

            GameObject.Destroy(this.gameObject, ExplosionSeconds);
        }
    }

    public void Shoot()
    {
        var missilePrefab = MissilePrefabs[Random.Range(0, MissilePrefabs.Count())];
        Vector3 shootPoint = transform.Find("ShootPoint").position;
        Instantiate(missilePrefab, shootPoint, Quaternion.identity, null);
    }
}
