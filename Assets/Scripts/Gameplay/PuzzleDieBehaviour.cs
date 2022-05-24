using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleDieBehaviour : MonoBehaviour
{
    public PuzzleDie PuzzleDie { get; private set; }

    public void Init(PuzzleDie puzzleDie)
    {
        this.PuzzleDie = puzzleDie;

        // Create appropriate graphics for die here
    }


}
