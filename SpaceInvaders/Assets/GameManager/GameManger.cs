using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManger : MonoBehaviour
{
    // Singleton instance
    public static GameManger instance;

    public int AlienCount = 50;
    public int AlienInitalCount = 50;

    public int MissilesSimultaneous = 3;

    public float MissileReloadMin = 1.2f;
    public float MissileReloadMax = 3f;

    internal float AnimationSpeedRatio = 0;

    private float AlienAnimationFPS = 3f;
    public float AlienAnimationFPSMin = 3f;
    public float AlienAnimationFPSMax = 9f;
    internal int AlienAnimationFrame = 0;
    private float AlienAnimationLastTime = 0;

    public List<float> MissileShootTimes = new List<float>();
    public float MissileReloadSeconds = 3;

    private Dictionary<string, AlienAnimation> Aliens = new Dictionary<string, AlienAnimation>();

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

    void Start()
    {
        CountAliens();
        for (int i = 0; i < MissilesSimultaneous; i++)
        {
            ReloadMissile();
        }
    }

    private void CountAliens()
    {
        Aliens.Clear();
        foreach (AlienAnimation alien in FindObjectsOfType<AlienAnimation>(false))
        {
            Aliens.Add(alien.gameObject.name, alien);
        }
        AlienInitalCount = Aliens.Count;
        AlienCount = AlienInitalCount;
    }

    public void AlienDied(AlienAnimation alien)
    {
        Aliens.Remove(alien.name);
        AlienCount--;
    }

    private void AlienShootRandom()
    {
        if (Aliens.Count > 0)
        {
            AlienAnimation alien = Aliens.ElementAt(Random.Range(0, Aliens.Count)).Value;
            alien.Shoot();
        }
    }

    // Update is called once per frame
    void Update()
    {
        AnimationSpeedRatio = Mathf.Clamp((Mathf.Sqrt(Mathf.Clamp(AlienCount, 1, 1000)) / Mathf.Sqrt(AlienInitalCount)), 0f, 1f);
        AlienAnimationFPS = SpeedFromAlienCount(AlienAnimationFPSMin, AlienAnimationFPSMax);
        MissileReloadSeconds = SpeedFromAlienCount(MissileReloadMax, MissileReloadMin);

        if (Time.time > AlienAnimationLastTime + (1 / AlienAnimationFPS))
        {
            AlienAnimationLastTime = Time.time;
            AlienAnimationFrame += 1;
        }

        foreach (float missileShootTime in MissileShootTimes.ToArray())
        {
            if (missileShootTime < Time.time)
            {
                MissileShootTimes.Remove(missileShootTime);
                //Debug.Log("Missile shooting " + missileShootTime.ToString() + " < " + Time.time.ToString() + "  of " + MissileShootTimes.Count().ToString());
                AlienShootRandom();
            }
        }
    }

    public float SpeedFromAlienCount(float min, float max)
    {
        return max - (max - min) * AnimationSpeedRatio;
    }

    public void MissileDestroyed()
    {
        ReloadMissile();
    }

    private void ReloadMissile()
    {
        float reloadTime = Random.Range(MissileReloadSeconds * .5f, MissileReloadSeconds * 1.5f);
        MissileShootTimes.Add(Time.time + reloadTime);
        //Debug.Log("Missile Reload " + reloadTime.ToString() + " -- " + (Time.time + reloadTime).ToString());
    }
}
