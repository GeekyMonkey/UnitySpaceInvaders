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


    public float PaddingX = 12;
    public float PaddingY = 12;
    public float ScreenXMin = -100;
    public float ScreenXMax = 100;
    public float ScreenYMin = -100;
    public float ScreenYMax = 100;

    public float SwarmDirectionX = 1;
    public float SwarmMoveStepX = 10;
    public float SwarmMoveStepY = 10;
    private float SwarmMoveNextTime = 1;
    public float SwarmMoveSecondsMax = 1.1f;
    public float SwarmMoveSecondsMin = 0.1f;

    public bool PlayerAlive = true;

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
        var bottomLeft = GameObject.Find("ScreenBorderBottomLeft").GetComponent<Transform>().position;
        var topRight = GameObject.Find("ScreenBorderTopRight").GetComponent<Transform>().position;
        ScreenXMin = bottomLeft.x;
        ScreenXMax = topRight.x;
        ScreenYMin = bottomLeft.y;
        ScreenYMax = topRight.y;
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

    public void PlayerDied(PlayerMovement player)
    {
        PlayerAlive = false;
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
        AnimationSpeedRatio = Mathf.Clamp((Mathf.Sqrt(Mathf.Clamp(AlienCount - 1, 0, 1000)) / Mathf.Sqrt(AlienInitalCount - 1)), 0f, 1f);
        AlienAnimationFPS = SpeedFromAlienCount(AlienAnimationFPSMin, AlienAnimationFPSMax);
        MissileReloadSeconds = SpeedFromAlienCount(MissileReloadMax, MissileReloadMin);

        // Animate aliens
        if (Time.time > AlienAnimationLastTime + (1 / AlienAnimationFPS))
        {
            AlienAnimationLastTime = Time.time;
            AlienAnimationFrame += 1;
        }

        if (PlayerAlive)
        {
            MoveSwarm();

            // Alien shooting
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
    }

    void MoveSwarm()
    {
        // Aliens won
        if (SwarmDirectionX == 0)
        {
            return;
        }

        if (Time.time > SwarmMoveNextTime)
        {
            float swarmMoveSeconds = SpeedFromAlienCount(SwarmMoveSecondsMax, SwarmMoveSecondsMin);
            // Debug.Log("Swarm move sec: " + swarmMoveSeconds.ToString());
            SwarmMoveNextTime = Time.time + swarmMoveSeconds;

            bool swarmHitLeft = false;
            bool swarmHitRight = false;
            bool swarmHitBottom = false;
            foreach (var alien in Aliens)
            {
                if (alien.Value.HitLeft && SwarmDirectionX == -1)
                {
                    swarmHitLeft = true;
                    break;
                }
                if (alien.Value.HitRight && SwarmDirectionX == 1)
                {
                    swarmHitRight = true;
                    break;
                }
                if (alien.Value.HitBottom)
                {
                    swarmHitBottom = true;
                }
            }

            float dx = 0;
            float dy = 0;
            if (swarmHitLeft || swarmHitRight)
            {
                SwarmDirectionX *= -1;
                dy = -SwarmMoveStepY;
            }
            else
            {
                dx = SwarmMoveStepX * SwarmDirectionX;
            }

            if (swarmHitBottom)
            {
                dx = 0;
                dy = 0;
                SwarmDirectionX = 0;
            }

            foreach (var alien in Aliens)
            {
                alien.Value.Move(dx, dy);
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
