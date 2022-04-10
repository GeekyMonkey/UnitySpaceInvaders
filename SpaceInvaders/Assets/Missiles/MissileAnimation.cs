using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;

public class MissileAnimation : MonoBehaviour
{
    public float AnimationFPS = 4;
    // public float ExplosionForce = 100f;
    // public float ExplosionSeconds = 6f;
    // public GameObject ExplosionPrefab;
    public float SpeedMin = 80;
    public float SpeedMax = 100;

    public float Life = 3;

    List<GameObject> AnimationFrames = new List<GameObject>();
    public GameObject[] AnimationFramePrefabs;
    private int FrameCount;
    private int FrameShown = -1;
    // private bool Dead = false;


    // Start is called before the first frame update
    void Start()
    {
        CreateAnimationFrames();
        ShowFrame(0);
        Destroy(gameObject, Life);
    }

    // Update is called once per frame
    void Update()
    {
        // if (Dead == false)
        // {
        ShowFrame((int)(Time.time * AnimationFPS) % FrameCount);
        // }
        Move();
    }

    void Move()
    {
        float speed = GameManger.instance.SpeedFromAlienCount(SpeedMin, SpeedMax);
                transform.Translate(0, -speed * Time.deltaTime, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Debug.Log("Missile Trigger " + other.tag);
        if (other.tag == "Bullet")
        {
            Destroy(gameObject);
            //   Die(other.transform.position);
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

    void CreateAnimationFrames()
    {
        for (int i = 0; i < AnimationFramePrefabs.Length; i++)
        {
            GameObject frame = Instantiate(AnimationFramePrefabs[i], transform.position, Quaternion.identity, this.transform);
            frame.SetActive(i == 0);
            AnimationFrames.Add(frame);
        }
        FrameCount = AnimationFrames.Count();
    }

    private void OnDestroy()
    {
        GameManger.instance.MissileDestroyed();
    }

    // void Die(Vector3 bulletPosition)
    // {
    //     if (!Dead)
    //     {
    //         Dead = true;
    //         GameManger.instance.AlienDied();

    //         var pixels = this.GetComponentsInChildren<PixelScript>(false);
    //         Vector3 explosionPoint = new Vector3(bulletPosition.x, bulletPosition.y, transform.Find("ExplosionPoint").position.z);
    //         foreach (var pixel in pixels)
    //         {
    //             pixel.ExplodeFrom(explosionPoint, ExplosionForce, ExplosionSeconds);
    //         }

    //         Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
    //         GetComponent<BoxCollider>().enabled = false;

    //         GameObject.Destroy(this.gameObject, ExplosionSeconds);
    //     }
    // }
}
