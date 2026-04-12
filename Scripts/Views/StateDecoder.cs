using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
                        Player       => "▼",
                        IInteractalbe tool  => DecodeTool(tool),
                        GoodsStock   => "G",
                        Table        => "T",
                        _            => ".",

                    };

                    string ontile = cell.Occupier?.OnHand switch
                    {
                        Ingredint ingredint => DecodeIngredient(ingredint),
                        ICarriable container => container switch
                        {
                            Plate => "d ",
                            Pot   => "P ",
                            _     => "? "
                        },
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



    private string DecodeIngredient (Ingredint ingredint)
    {
        return ingredint.Type switch
        {
            IngredintType.ONION  => "o ",
            IngredintType.TOMATO => "t ",
            _ => "?"
        };
    }
    private string DecodeTool(IInteractalbe stove)
    {
        string color = stove.ToolState switch
        {
            State.BROKEN => "red",
            State.ON     => "green",
            _            => "gray"
            
        };
        return $"[color={color}]{stove.ViewId}[/color]";
    }


    public string DecodeClock(GameState state)
    {
        string mainClock = "Clock: " + new string('.', state.Tics);
        string statoinsTimers ="";

        foreach(var station in state.WorkStations)
        {
            var progress = station.GetProgress();
            if (progress == null) return mainClock + statoinsTimers;;

            int value = progress.Sum(it => it.Cooking);
            string color = progress?.LastOrDefault().Grade switch
            {
                CookGrade.Cooked => "green",
                CookGrade.OverCooked => "red",
                _ => "white",
            }?? "white";

            statoinsTimers += $"\n{station.GetType}: [color={color}]{ new string('.', value)}[/color]";
        }
        return mainClock + statoinsTimers;
    }

    public List<string> DecodePots(List<Pot> pots)
    {
        List<string> output = [];
        foreach(var pot in pots)
        {
            string potString = $"pot: ";
            foreach (var ingredien in pot.GetIngredients())
            {
                char symbol = ingredien.Type switch
                {
                    IngredintType.ONION  => 'o',
                    IngredintType.TOMATO => 't',
                    _                    => '.',
                };
                potString += symbol + ", ";
            }
            output.Add(potString);
        }
        return output;
    }
}