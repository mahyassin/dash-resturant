using System.Collections.Generic;
using System.Linq;
using Godot;

namespace Scripts.Models
{   
    public interface ICarriable
    {
        int CapablityCode {get;}
    };
    public interface IOnplate;
    public interface IChoppable;

    public class Plate: ICarriable
    {
        public int CapablityCode => Varifier.Platable;

        public class IOnplate;
    }

    public class Ingredint: ICarriable, IChoppable
    {
        public IngredintType Type {get;}

        public Ingredint(IngredintType type)
        {
            Type = type;
        }
        public int CoockProgression = 0;
        public int ChoppingProgression = 0;
        public int MaxCocking => 5;
        public int MaxChopping =>5;

        public int CapablityCode => Varifier.Platable | Varifier.Washable | Varifier.Choppable | Varifier.Coockable;
    }
    public enum IngredintType
    {
        ONION,
        TOMATO,
    }
    

    public static class IdRegestiry
    {
        static public HashSet<int> ContainersId = new(){0};
        
    }

    public class Pot: ICarriable
    {
        private List<Ingredint> _ingredints = new();

        public int CapablityCode => Varifier.Coockable;

        public void AddIngredient(Ingredint ingredint)
        {
            _ingredints.Add(ingredint);
            GD.Print($"added count is {_ingredints.Count}");
        }
        
        public List<Ingredint> GetIngredients()
        {
            return _ingredints;
        }
    }
}