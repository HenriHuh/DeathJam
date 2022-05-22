using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// One of the sides of a die for gameplay
/// </summary>
public class PuzzleDie
{
    public Vector2Int Position { get; private set; }
    private int currentSideIndex;
    private SO_PuzzleDie asset;


    public SO_PuzzleDieSide CurrentSide => asset.sides[currentSideIndex];

    public PuzzleDie(SO_PuzzleDie asset)
    {
        this.asset = asset;
    }

    public void SetPosition(Vector2Int position)
    {
        Position = position;
    }

    public SO_PuzzleDieSide Roll()
    {
        currentSideIndex = Random.Range(0, asset.sides.Length);
        return CurrentSide;
    }

}
