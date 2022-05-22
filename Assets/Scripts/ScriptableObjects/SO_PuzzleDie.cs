using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SO_PuzzleDie : MonoBehaviour
{
    public SO_PuzzleDieSide[] sides;

    public SO_PuzzleDieSide Roll()
    {
        return sides[Random.Range(0, sides.Length)];
    }
}
