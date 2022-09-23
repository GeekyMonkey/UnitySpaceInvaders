using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManger : MonoBehaviour
{
    // Singleton instance
    public static GameManger instance;

    public int AlienCount = 0;
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

    private List<float> previousShootX = new List<float>();

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

    private int UfoSecondsMin = 8;
    private int UfoSecondsMax = 36;

    private int Score = 0;
    public int Level = 1;
    public float LevelMultipler = 7f;
    public Vector3 SwarmStartPosition = new Vector3(-75, 0, 0);

    public bool PlayerAlive = true;

    public TMPro.TextMeshProUGUI LevelValueText;
    public TMPro.TextMeshProUGUI ScoreValueText;
    public TMPro.TextMeshProUGUI PlayerNameText;

    private Dictionary<string, AlienAnimation> Aliens = new Dictionary<string, AlienAnimation>();
    public GameObject Swarm;

    public List<GameObject> AlienSwarms = new List<GameObject>();
    public List<GameObject> UfoTypes = new List<GameObject>();
    public GameObject Player;
    public Transform UfoStartPosition;
    public int UfoType = 1;

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

    IEnumerator Start()
    {
        var bottomLeft = GameObject.Find("ScreenBorderBottomLeft").GetComponent<Transform>().position;
        var topRight = GameObject.Find("ScreenBorderTopRight").GetComponent<Transform>().position;
        ScreenXMin = bottomLeft.x;
        ScreenXMax = topRight.x;
        ScreenYMin = bottomLeft.y;
        ScreenYMax = topRight.y;
        PlayerNameText.text = GlobalStateScript.Instance.PlayerName;
        LevelValueText.text = Level.ToString();

        DestroySwarm();
        yield return new WaitForSeconds(3f);
        SpawnSwarm();
    }

    void DestroySwarm()
    {
        AlienCount = 0;
        if (Swarm != null)
        {
            Destroy(Swarm);
            Swarm = null;
        }
    }

    void SpawnSwarm()
    {
        Swarm = Instantiate(AlienSwarms[(Level - 1) % AlienSwarms.Count], SwarmStartPosition, Quaternion.identity);
        CountAliens();
        for (int i = 0; i < MissilesSimultaneous; i++)
        {
            ReloadMissile();
        }
        SpawnUfo();
    }

    async void SpawnUfo()
    {
        await Task.Delay(Random.Range(UfoSecondsMin * 1000, UfoSecondsMax * 1000));
        if (AlienCount > 0 && EditorApplication.isPlaying)
        {
            Debug.Log("Spawn ufo type " + UfoType);
            GameObject Ufo = Instantiate(UfoTypes[UfoType % UfoTypes.Count], UfoStartPosition.position, Quaternion.identity);
            UfoType++;
            SpawnUfo();
        }
    }

    private void CountAliens()
    {
        Aliens.Clear();
        foreach (AlienAnimation alien in FindObjectsOfType<AlienAnimation>(false))
        {
            // Ignore ufos               
            if (alien.gameObject.CompareTag("Alien"))
            {
                Aliens.Add(alien.gameObject.name, alien);
            }
        }
        AlienInitalCount = Aliens.Count;
        AlienCount = AlienInitalCount;
        Debug.Log("Alien count=" + AlienCount);
    }

    public void AlienDied(AlienAnimation alien)
    {
        Score += alien.Points;
        ScoreValueText.text = Score.ToString();

        // Skip for UFO
        if (alien.gameObject.CompareTag("Alien"))
        {
            Aliens.Remove(alien.name);
            AlienCount--;
            Debug.Log("AlienCount:" + AlienCount.ToString());
            if (AlienCount < 1)
            {
                LevelUp();
            }
        }
    }

    private async void LevelUp()
    {
        Level++;
        LevelValueText.text = Level.ToString();
        DestroySwarm();
        await Task.Delay(2000);
        SpawnSwarm();
    }

    public async void PlayerDied(PlayerMovement player)
    {
        PlayerAlive = false;
        GlobalStateScript.Instance.AddHighScore(GlobalStateScript.Instance.PlayerName, Score);

        await Task.Delay(3000);

        SceneManager.LoadScene("MenuScene", LoadSceneMode.Single);
    }

    private bool AlienShootRandom()
    {
        bool didShoot = false;

        if (Aliens.Count > 0)
        {
            AlienAnimation alien = Aliens.ElementAt(Random.Range(0, Aliens.Count)).Value;

            // Prevent multiple missiles from same X
            float shootX = alien.transform.position.x;
            if (!previousShootX.Any((x) => x == shootX))
            {
                alien.Shoot();
                didShoot = true;
                previousShootX.Add(shootX);
                while (previousShootX.Count() > MissilesSimultaneous)
                {
                    previousShootX.Remove(previousShootX[0]);
                }
            }
        }

        return didShoot;
    }

    // Update is called once per frame
    void Update()
    {
        AnimationSpeedRatio = Mathf.Clamp((Mathf.Sqrt(Mathf.Clamp(AlienCount - 1, 0, 1000)) / Mathf.Sqrt(AlienInitalCount - 1)), 0f, 1f);
        AlienAnimationFPS = SpeedFromAlienCount(AlienAnimationFPSMin, AlienAnimationFPSMax, Level);
        MissileReloadSeconds = SpeedFromAlienCount(MissileReloadMax, MissileReloadMin, 1);

        // Animate aliens
        // if (Time.time > AlienAnimationLastTime + (1 / AlienAnimationFPS))
        // {
        //     AlienAnimationLastTime = Time.time;
        //     AlienAnimationFrame += 1;
        // }

        if (PlayerAlive)
        {
            MoveSwarm();

            // Alien shooting
            foreach (float missileShootTime in MissileShootTimes.ToArray())
            {
                if (missileShootTime < Time.time)
                {
                    //Debug.Log("Missile shooting " + missileShootTime.ToString() + " < " + Time.time.ToString() + "  of " + MissileShootTimes.Count().ToString());
                    if (AlienShootRandom())
                    {
                        MissileShootTimes.Remove(missileShootTime);

                    }
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
            AlienAnimationFrame += 1;
            float swarmMoveSeconds = SpeedFromAlienCount(SwarmMoveSecondsMax, SwarmMoveSecondsMin, Level);
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
                Player.GetComponent<PlayerMovement>().Die(Player.transform.position);
            }

            foreach (var alien in Aliens)
            {
                alien.Value.Move(dx, dy);
            }
        }
    }

    /** Delay in seconds */
    public float SpeedFromAlienCount(float min, float max, int level)
    {
        var s = max - (max - min) * AnimationSpeedRatio;
        if (level != 1) {
            s /= (((float)Level - 1.0f) * LevelMultipler + 1);
        }
        return s;
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
