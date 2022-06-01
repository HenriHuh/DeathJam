using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// One of the sides of a die for gameplay
/// </summary>
public class PuzzleDie
{
    public GameObject GameObject { get; private set; }
    public Vector2Int Position { get; private set; }
    public Vector3 TransformPosition => GameObject.transform.position;
    public int CurrentSideIndex { get; private set; }
    private SO_PuzzleDie asset;
    public SO_PuzzleDieSide CurrentSide => asset.sides[CurrentSideIndex];


    public PuzzleDie(SO_PuzzleDie asset, GameObject gameObject = null)
    {
        this.asset = asset;
        this.GameObject = gameObject;
    }

    public void SetPosition(Vector2Int position)
    {
        Position = position;
    }

    public SO_PuzzleDieSide Roll()
    {
        int nextSideIndex = Random.Range(0, asset.sides.Length - 1);
        // Prevent die from landing on the same side on wich it started from
        CurrentSideIndex = nextSideIndex + (nextSideIndex >= CurrentSideIndex ? 1 : 0);

        return CurrentSide;
    }

}
