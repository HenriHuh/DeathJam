using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Handles most of the gameplay logic
/// </summary>
public class PuzzleController
{


    private int gridSizeX;
    private int gridSizeY;
    public PuzzleDie[] dice;

    public delegate void Delegate_Match(List<PuzzleDie> dice);
    public delegate void Delegate_Roll(PuzzleDie die);
    public Delegate_Match MatchDelegate { get; set; }
    public Delegate_Roll RollDelegate { get; set; }


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
            RollDelegate.Invoke(dice[i]);
            dice[i].Roll();
        }
    }

    public PuzzleDie FindDie(Vector2Int position)
    {
        for (int i = 0; i < dice.Length; i++)
        {
            if(dice[i].Position == position)
            {
                dice[i].Roll();
                return dice[i];
            }
        }

        Debug.LogError("No die found at position " + position);
        return null;
    }

    public void Roll(PuzzleDie die)
    {
        die.Roll();
        RollDelegate.Invoke(die);
    }

    public void Roll(List<PuzzleDie> dice)
    {
        for (int i = 0; i < dice.Count; i++)
        {
            Roll(dice[i]);
        }
    }

    /// <summary>
    /// Returnds dice that should be rerolled
    /// </summary>
    public List<PuzzleDie> CheckAllMatches()
    {

        List<PuzzleDie> allMatches = new List<PuzzleDie>();
        for (int i = 0; i < dice.Length; i++)
        {
            for (int j = 0; j < dice[i].CurrentSide.types.Count; j++)
            {
                List<PuzzleDie> connected = GetConnectedDice(dice[i], dice[i].CurrentSide.types[j]);
                if(connected.Count >= 3)
                {
                    AddRangeDistinct(allMatches, connected);
                }
            }
        }
        return allMatches;
    }

    private static void AddRangeDistinct(List<PuzzleDie> allMatches, List<PuzzleDie> connected)
    {
        for (int k = 0; k < connected.Count; k++)
        {
            if (!allMatches.Contains(connected[k]))
            {
                allMatches.Add(connected[k]);
            }
        }
    }

    private bool TryGetDie(Vector2Int position, out PuzzleDie die)
    {
        for (int i = 0; i < dice.Length; i++)
        {
            if (dice[i].Position == position)
            {
                die = dice[i];
                return true;
            }
        }

        die = null;
        return false;
    }

    public List<PuzzleDie> GetConnectedDice(PuzzleDie die, Enums.ElementalType type)
    {
        // Too tired to think of a better algorithm...

        List<PuzzleDie> connected = new List<PuzzleDie>();
        List<PuzzleDie> closed = new List<PuzzleDie>();
        List<PuzzleDie> open = new List<PuzzleDie>();
        connected.Add(die);
        open.Add(die);

        while (open.Count > 0)
        {
            PuzzleDie current = open[0];
            for (int i = 0; i < Tools.sides.Length; i++)
            {
                if(TryGetDie(current.Position + Tools.sides[i], out PuzzleDie connectedDie) 
                    && connectedDie.CurrentSide.CheckMatch(type) 
                    && !open.Contains(connectedDie) 
                    && !closed.Contains(connectedDie))
                {
                    open.Add(connectedDie);
                    connected.Add(connectedDie);
                }

            }
            open.Remove(current);
            closed.Add(current);
        }

        return connected;
    }
}
