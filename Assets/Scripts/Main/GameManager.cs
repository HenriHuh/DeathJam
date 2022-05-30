using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<SO_PuzzleDie> diceAssets;
    public GameObject board;

    public static GameManager instance;
    public float diceScaleFactor;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("FIXME: Removed unnecessary GameManager!");
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

}
