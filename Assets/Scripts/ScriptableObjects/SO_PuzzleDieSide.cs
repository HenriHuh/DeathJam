using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SO_PuzzleDieSide : ScriptableObject
{
    [Tooltip("Which other elements can this side match with")] 
    public List<Enums.ElementalType> types;
    public Sprite image;


    public bool CheckMatch(Enums.ElementalType otherType)
    {
        if (types.Contains(otherType)) return true;
        return false;
    }


}
