using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<SO_PuzzleDie> diceAssets;
    public GameObject board;
    public GameObject victoryDisco;

    public Sprite sprite_ElementMind;
    public Sprite sprite_ElementSoul;
    public Sprite sprite_ElementHeart;
    public Sprite sprite_ElementWeird;

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
