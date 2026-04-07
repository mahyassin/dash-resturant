using Godot;
using System;
using System.Collections.Generic;

namespace Scripts.Models
{   
    public class Ingredint: Onhand
    {
        public IngredintType Type {get;}

        public Ingredint(IngredintType type)
        {
            Type = type;
        }

    }
    public enum IngredintType
    {
        ONION,
        TOMATO,
    }

    public class Dish
    {
        private List<Ingredint> _ingredints;

        public void AddIngredent(Ingredint ingredint)
        {
            _ingredints.Add(ingredint);
        }
    }


    public class Plate: Onhand
    {
        public Dish OnPlate;
        public int DirtLevel = 5;
    }

    public class Pot: Onhand
    {
        public List<Ingredint> Ingredints;
    }

    public abstract class Onhand
    {
        public Dictionary<ProgressType, int> Progress = new()
        {
            {ProgressType.ACTIVE, 0},
            {ProgressType.PASSIVE, 0},
        };
    }

    public enum ProgressType 
    {
        PASSIVE,
        ACTIVE,
    }

}