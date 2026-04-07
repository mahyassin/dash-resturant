using System;
using Scripts.Models;

public partial class StateDecoder
{
     public string[] DecodeState(GameState state)
    {
        string[] output = new string[state.Maphieght];
            
        for (int y = 0; y < state.Maphieght; y++) // y is the outer loop (rows)
        {
            for (int x = 0; x < state.MapWidth; x++) // x is the inner loop (columns)
            {
                // Use new(x, y) to match how you stored it!
                if (state.Map.TryGetValue(new Vector(x, y), out var cell))
                {
                    string basetile = cell.Occupier switch
                    {
                        Player       => "P",
                        WorkingStation tool  => DecodeTool(tool),
                        GoodsStock   => "G",
                        Table        => "T",
                        _            => ".",

                    };

                    string ontile = cell.Occupier?.OnHand switch
                    {
                        Ingredint ingredint => "o ",
                        _=> ". "
                        
                    };

                    string reusult = cell.Occupier is Wall ? "WW " : basetile + ontile;
                    output[y] += reusult  ;
                }
                else
                {
                    output[y] += '!'; // Debug character if a coordinate is missing
                }
            }
        }
        return output;
    } 
    private string DecodeTool(WorkingStation stove)
    {
        string color = stove.StationState switch
        {
            WorkingStation.State.BROKEN => "red",
            WorkingStation.State.ON     => "green",
            _                         => "gray"
            
        };
        return $"[color={color}]{stove.ViewId}[/color]";
    }

    private string DecodeOnHand(Ingredint ingredint)
    {
        return ingredint.Type switch
        {
            IngredintType.ONION  
            => "o ",
            IngredintType.TOMATO => "t ",
            _                    => " .",
        };
    }


    public string DecodeClock(GameState state)
    {
        string mainClock = "Clock: " + new string('.', state.Tics);
        string statoinsTimers ="";

        foreach(var station in state.WorkStations)
        {
            var progress = station.OnHand?.Progress;

            statoinsTimers += $"\n{station.StationType}: { new string('.', progress?[station.progressType]?? 0) }";
        }
        return mainClock + statoinsTimers;
    }
}