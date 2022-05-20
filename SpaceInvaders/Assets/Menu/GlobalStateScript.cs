using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalStateScript : MonoBehaviour
{
    public static GlobalStateScript Instance;

    internal string PlayerName = "Player A";

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

}
