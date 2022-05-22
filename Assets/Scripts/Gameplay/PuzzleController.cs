using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles most of the gameplay logic
/// </summary>
public class PuzzleController
{

    private int gridSizeX;
    private int gridSizeY;
    public PuzzleDie[] dice;


    public PuzzleController(int gridSizeX, int gridSizeY, PuzzleDie[] dice)
    {
        this.gridSizeX = gridSizeX;
        this.gridSizeY = gridSizeY;

        if(gridSizeX * gridSizeY < dice.Length)
        {
            Debug.LogError("Grid size is too small for dice.");
        }

        this.dice = dice;
        for (int i = 0; i < this.dice.Length; i++)
        {
            Vector2Int position = new Vector2Int((int)Mathf.Repeat(i, gridSizeX), i / gridSizeY); // !! Check that this is correct
            this.dice[i].SetPosition(position);
        }
    }

    public void RollAll()
    {
        for (int i = 0; i < dice.Length; i++)
        {
            dice[i].Roll();
        }
    }

    public void Roll(Vector2Int position)
    {
        for (int i = 0; i < dice.Length; i++)
        {
            if(dice[i].Position == position)
            {
                dice[i].Roll();
                return;
            }
        }

        Debug.LogError("No die found at position " + position);
    }
}
