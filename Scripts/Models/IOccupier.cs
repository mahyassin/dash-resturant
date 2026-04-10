using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Godot;

namespace Scripts.Models
{

    public interface IProgressor
    {
        public void ProgressPassive();
        public void ProgressActive();

        public List<Progress> GetProgress();

    };
    public interface IInteractalbe: IProgressor
    {
        public State ToolState{get; set;}
        public void Interact();
        public char ViewId {get;}
    };

    public enum State
    {
        ON, OFF, BROKEN
    }

  
    

	public abstract class IOccupier{
        
		public Vector Pos {get; set;}
        public ICarriable OnHand {get;set;}
        public abstract int AbilityCode {get;}

	}


	public class Player: IOccupier
	{
        public Player(Vector pos, ICarriable onhand)
        {
			Pos = pos;
            OnHand = onhand;
		}

        public override int AbilityCode => 0;
    }

    public class Stove: IOccupier, IInteractalbe, IProgressor
    {
        public State ToolState {get; set;} = State.OFF;

        public char ViewId => 'S';

        public override int AbilityCode => Varifier.Coockable;

        public int TotalProgression;

        public void Interact()
        {
            ToolState = ToolState switch
            {
                State.ON     => State.OFF,
                State.OFF    => State.ON,
                _            => ToolState
            };
        }

        public void ProgressPassive()
        {
            if (OnHand is not Pot onStove) {  return; }
            if (onStove.GetIngredients().Count <= 0) { return;}


            foreach (var ingredien in onStove.GetIngredients())
            {
                var progress = ingredien.progress;
            
                if(progress.Cooking >= progress.MaxCocking) continue;

                ingredien.progress = new(progress.Choping++, progress.Choping);

                return;
            }
        }

        public void ProgressActive()
        {
            
        }

        public List<Progress> GetProgress()
        {
            if(OnHand == null) return null;


            List<Progress> progress = (OnHand as Pot).GetIngredients().Select(ingredint => ingredint.progress).ToList();

            return progress;
        }


        public Stove()
        {
            OnHand = new Pot();
        }
    }

    public class CuttingBoard(Ingredint choppable): IOccupier, IInteractalbe
    {
        public Ingredint OnBoard = choppable;

        public State ToolState {get; set;} = State.OFF;

        public char ViewId => 'C';

        public override int AbilityCode => Varifier.Choppable;

        public void Interact()
        {
            ToolState = ToolState switch
            {
                State.ON     => State.OFF,
                State.OFF    => State.ON,
                _            => ToolState
            };
        }
          public void ProgressPassive()
        {
            
        }

        public void ProgressActive()
        {
            if(OnBoard == null) return;
            
            int cp = OnBoard.progress.Choping;

            cp = cp  >= OnBoard.progress.MaxChopping? cp: cp + 1;

            OnBoard.progress.Choping = cp;

            Interact();
        }

        public List<Progress> GetProgress()
        {
            if(OnBoard == null) return new();
            return new(){OnBoard.progress};
        }
    }


    public class GoodsStock: IOccupier
    {
        private int _currentStock;
        public Ingredint Ingredint { get ; set; }
        public IngredintType Type {get;}

        public override int AbilityCode => Varifier.Choppable | 
            Varifier.Coockable | 
            Varifier.Platable  | 
            Varifier.Washable
        ;

        public GoodsStock(Ingredint onhand)
        {
            OnHand = onhand;
            Type = onhand.Type;

            _currentStock = 3;

        }

        public void Reduce()
        {
            _currentStock--;

            GD.Print(_currentStock);    

            if(_currentStock <= 0)
            {
                OnHand = null;
                return;
            }

            OnHand = new Ingredint(Type);
        }
    }


    public class Wall : IOccupier
    {
        public override int AbilityCode => 0;
    }

    public class Table : IOccupier
    {
        public Table(ICarriable onhand)
        {
            OnHand = onhand;
        }

        public override int AbilityCode => Varifier.Choppable | 
            Varifier.Coockable | 
            Varifier.Platable  | 
            Varifier.Washable
            ;
    }

}