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

    private float xMin = 0;
    private float xMax = 100;
    private float y = -50;
    private PlayerInput playerInput;
    private Transform CanonPosition;

    // Start is called before the first frame update
    void Start()
    {
        xMin = GameObject.Find("ScreenBorderBottomLeft").GetComponent<Transform>().position.x + PlayerWidth / 2;
        xMax = GameObject.Find("ScreenBorderTopRight").GetComponent<Transform>().position.x - PlayerWidth / 2;
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
        MoveInput();
        FireInput();
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
}
