using System.Security.Cryptography.X509Certificates;
using Godot;

namespace Scripts.Models
{
	public abstract class IOccupier{
		public Vector Pos {get; set;}
        public Onhand OnHand {get;set;}
        public abstract bool Stacker {get;}
        public int MaxStack => 4;
 
	}


	public class Player: IOccupier
	{
        public override bool Stacker => true;
        public Player(Vector pos, Onhand onhand)
        {
			Pos = pos;
            OnHand = onhand;
		}

    }

	public class WorkingStation: IOccupier, IInteractalbe
	{
        public override bool Stacker => true;

        public ProgressType progressType{get;}

        
        public enum State
        {
            ON, OFF, BROKEN
        }
        public enum Type
        {
            STOVE, CUTTING_BOARD, DISH_WAHSER,
        }
        public Type StationType {get;}
        public char ViewId {get;}
        private State _state = State.OFF;
        public State StationState => _state;
		
        public void Interact()
        {
            if(_state == State.BROKEN) return;
            if(_state == State.ON) {_state = State.OFF; return;}
            if(_state == State.OFF) {_state = State.ON; }

        }

        public WorkingStation(Type type, Onhand onhand)
        {
            StationType = type;
            OnHand = onhand;

            if(type == Type.CUTTING_BOARD)
            {
                ViewId = 'C';
                progressType = ProgressType.ACTIVE;
            }

             if(type == Type.DISH_WAHSER)
            {
                ViewId = 'D';
                progressType = ProgressType.ACTIVE;
            } 
            
            if(type == Type.STOVE)
            {
                ViewId = 'S';
                progressType = ProgressType.PASSIVE;
                
            }
        }
    }

    public class GoodsStock: IOccupier
    {
        public override bool Stacker => true;
        private int _currentStock;
        public Ingredint Ingredint { get ; set; }
        public IngredintType Type {get;}


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


    public class Wall: IOccupier
    {
        public override bool Stacker => false;
    }

    public class Table : IOccupier
    {
        public override bool Stacker => true;
    }

    public interface IInteractalbe
    {
        public void Interact();
    }

}