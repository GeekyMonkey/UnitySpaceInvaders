using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float PlayerWidth = 13.0f;
    public float Speed = 30f;
    public float ReloadTime = 0.5f;
    public float LastShotTime = 0f;
    public GameObject BulletPrefab;

    public GameObject ExplosionPrefab;


    public float ExplosionForce = 100f;
    public float ExplosionSeconds = 6f;

    public bool Dead = false;

    private float xMin = 2;
    private float xMax = 200;
    private float y = -50;
    private PlayerInput playerInput;
    private Transform CanonPosition;

    // Start is called before the first frame update
    void Start()
    {
        xMin = GameManger.instance.ScreenXMin + PlayerWidth / 2;
        xMax = GameManger.instance.ScreenXMax - PlayerWidth / 2;
        y = transform.position.y;
        CanonPosition = transform.Find("CanonPosition");
    }

    void Awake()
    {
        playerInput = new PlayerInput();
    }

    void OnEnable()
    {
        playerInput.Enable();
    }

    void OnDisable()
    {
        playerInput.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Dead)
        {
            MoveInput();
            FireInput();
        }
    }

    void Fire()
    {
        GameObject.Instantiate(BulletPrefab, CanonPosition.position, Quaternion.identity);
    }

    void FireInput()
    {
        if (playerInput.Player.Fire.IsPressed())
        {
            if (LastShotTime < Time.time - ReloadTime)
            {
                LastShotTime = Time.time;
                Fire();
            }
        }
    }

    void MoveInput()
    {
        //float ix = Input.GetAxis("Horizontal");
        float ix = playerInput.Player.Move.ReadValue<Vector2>().x;
        float dx = ix * Speed * Time.deltaTime;
        float x = transform.position.x + dx;
        x = Mathf.Clamp(x, xMin, xMax);
        transform.position = new Vector3(x, y, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Missile")
        {
            Die(other.transform.position);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Alien")
        {
            Die(other.transform.position);
        }
    }

    void Die(Vector3 bulletPosition)
    {
        if (!Dead)
        {
            Dead = true;
            // GameManger.instance.PlayerDied(this);

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
}
