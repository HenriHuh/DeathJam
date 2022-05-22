using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<SO_PuzzleDie> diceAssets;

    public static GameManager instance;

    void Start()
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

    void StartLevel( /*Insert level asset here*/)
    {

    }

}
