using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleDieBehaviour : MonoBehaviour
{
    public PuzzleDie PuzzleDie { get; private set; }
    public DiceTumble DiceTumble => GetComponentInChildren<DiceTumble>();
    public void Init(PuzzleDie puzzleDie)
    {
        this.PuzzleDie = puzzleDie;

        gameObject.SetActive(false);
    }

}
