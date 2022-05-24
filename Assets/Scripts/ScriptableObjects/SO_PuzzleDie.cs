using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PuzzleDie_Default", menuName = "ScriptableObjects/PuzzleDie")]
public class SO_PuzzleDie : ScriptableObject
{
    public SO_PuzzleDieSide[] sides;

    public SO_PuzzleDieSide Roll()
    {
        return sides[Random.Range(0, sides.Length)];
    }
}
