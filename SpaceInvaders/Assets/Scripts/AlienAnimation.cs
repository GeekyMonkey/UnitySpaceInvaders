using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;

public class AlienAnimation : MonoBehaviour
{
    public float AnimationSpeed = .6f;
    public float ExplosionForce = 100f;
    public float ExplosionSeconds = 6f;

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
            ShowFrame(((int)(Time.time * AnimationSpeed) % FrameCount));

            // todo - remove this 
            if (Time.time > 2 && this.gameObject.name == "Alien3 A")
            {
                Die();
            }
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

    async void Die()
    {
        if (!Dead)
        {
            Dead = true;
            var pixels = this.GetComponentsInChildren<PixelScript>(false);
            Vector3 explosionPoint = transform.Find("ExplosionPoint").position;
            foreach (var pixel in pixels)
            {
                pixel.ExplodeFrom(explosionPoint, ExplosionForce, ExplosionSeconds);
            }

            await Task.Delay((int)(ExplosionSeconds * 1000f));
            GameObject.Destroy(this.gameObject);
        }
    }
}
