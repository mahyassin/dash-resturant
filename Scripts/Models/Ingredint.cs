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

    public struct Progress
    {
        public int Cooking = 0;
        public int Choping = 0;
        public int MaxCocking => 30;
        public int MaxChopping =>5;
        public CookGrade Grade =>  Cooking switch
        {
            <= 25 and > 15 => CookGrade.Cooked,
            >  25          => CookGrade.OverCooked,
            _              => CookGrade.Raw,
        };

        public Progress( int cook, int chop)
        {
            Cooking = cook;
            Choping = chop;

        }
    }
    public class Ingredint: ICarriable, IChoppable
    {
        public IngredintType Type {get;}

        public Ingredint(IngredintType type)
        {
            Type = type;
        }
        public Progress progress = new();
       
        public int CapablityCode => Varifier.Platable | Varifier.Washable | Varifier.Choppable | Varifier.Coockable;
    }

    public enum IngredintType
    {
        ONION,
        TOMATO,
    }
    
    public enum CookGrade
    {
        Raw, MeduimRaw, Cooked, OverCooked,
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