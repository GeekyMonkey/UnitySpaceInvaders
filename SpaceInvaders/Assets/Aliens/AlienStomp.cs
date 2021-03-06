using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienStomp : MonoBehaviour
{

    public AudioClip[] AlienStompSounds;
    private int AlienStompSoundCount;
    private float AlienStompPerSecond = 1f;
    public float MaxSpeed = 11;
    public float MinSpeed = 1;

    private AudioSource StompAudioSource;
    private int StompIndex = -1;
    private float LastStompTime = 0;

    private int previousAlienAnimationFrame = 0;

    void Awake()
    {
        StompAudioSource = GetComponent<AudioSource>();
        AlienStompSoundCount = AlienStompSounds.Length;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManger.instance.AlienCount > 0 && GameManger.instance.PlayerAlive)
        {
            // AlienStompPerSecond = GameManger.instance.SpeedFromAlienCount(MinSpeed, MaxSpeed);
            // if (Time.time > LastStompTime + (1 / AlienStompPerSecond))
            // {
            //     LastStompTime = Time.time;
            //     PlayStomp();
            // }

            if (GameManger.instance.AlienAnimationFrame != previousAlienAnimationFrame)
            {
                previousAlienAnimationFrame = GameManger.instance.AlienAnimationFrame;
                PlayStomp();
            }
        }
    }

    void PlayStomp()
    {
        StompIndex += 1;
        if (StompIndex >= AlienStompSoundCount)
        {
            StompIndex = 0;
        }
        StompAudioSource.PlayOneShot(AlienStompSounds[StompIndex]);
    }
}
