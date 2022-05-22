using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles player inputs and manages visual behaviours
/// </summary>
public class LevelManager : MonoBehaviour
{
    PuzzleController controller;
    public int gridSizeX;
    public int gridSizeY;


    private void Start()
    {
        controller = new PuzzleController(gridSizeX, gridSizeY, CreateDice(8));
    }

    /// <summary>
    /// Should replace with some proper logic instead of just randomizing
    /// </summary>
    private PuzzleDie[] CreateDice(int count)
    {
        PuzzleDie[] dice = new PuzzleDie[count];
        for (int i = 0; i < count; i++)
        {
            SO_PuzzleDie asset = GameManager.instance.diceAssets[Random.Range(0, GameManager.instance.diceAssets.Count)];
            dice[i] = new PuzzleDie(asset);
        }
        return dice;
    }

}
