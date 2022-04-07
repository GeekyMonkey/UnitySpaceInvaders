using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManger : MonoBehaviour
{
    // Singleton instance
    public static GameManger instance;

    public int AlienCount = 50;
    public int AlienInitalCount = 50;
            
    internal float AnimationSpeedRatio = 0;

    private float AlienAnimationFPS = 3f;
    public float AlienAnimationFPSMin = 3f;
    public float AlienAnimationFPSMax = 9f;
    internal int AlienAnimationFrame = 0;
    private float AlienAnimationLastTime = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(this.gameObject);
            Init();
        }
        else
        {
            Destroy(this);
        }
    }

    void Init()
    {
    }
     
     void Start() {
         CountAliens();
     }                 

     private void CountAliens() {
        AlienInitalCount = FindObjectsOfType<AlienAnimation>(false).Length;
        AlienCount = AlienInitalCount;
     }

    public void AlienDied()
    {
        AlienCount--;
    }

    // Update is called once per frame
    void Update()
    {
        AnimationSpeedRatio = Mathf.Clamp((Mathf.Sqrt(Mathf.Clamp(AlienCount, 1, 1000)) / Mathf.Sqrt(AlienInitalCount)), 0f, 1f);
        AlienAnimationFPS = AlienAnimationFPSMax - (AlienAnimationFPSMax - AlienAnimationFPSMin) * AnimationSpeedRatio;

        if (Time.time > AlienAnimationLastTime + (1 / AlienAnimationFPS))
        {
            AlienAnimationLastTime = Time.time;
            AlienAnimationFrame += 1;
        }
    }
}
