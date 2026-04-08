using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

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
    

    public static class IdRegestiry
    {
        static public HashSet<int> ContainersId = new(0);
        
    }

    public class Container: Onhand
    {
        public int Id;
        public List<Ingredint> Ingredints;
        public ContainerType Type;
        public int DirtLevel = 5;

        public bool CanCoock {get;}
        public bool IsStackable {get;}

        public Container(bool canCoock, bool isStackable, ContainerType type)
        {
            CanCoock = canCoock;
            IsStackable = isStackable;
            Id = IdRegestiry.ContainersId.Max() + 1;
            Type = type;
        }
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

    public enum ContainerType
    {
        DISH,
        POT,
    }

}