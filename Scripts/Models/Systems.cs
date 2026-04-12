using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Scripts.Models
{
	

	public partial class Systems
	{
		public event Action<GameState> MapChanged;
		public event Action<GameState> ClockTiced;
		public event Action<string> Error;

		public void Move(IOccupier entity, GameState state, Vector dir )
		{
			Vector target = entity.Pos.Add(new(dir.X, -dir.Y));
			if (!state.Map.ContainsKey(target)) { Error.Invoke("out of Bound") ;return; }

			if (state.Map[target].IsWalkable) { MoveEntity(state, target, entity); }
			else { Debug.WriteLine("not Walkable");}

			if (state.Map[target].Occupier is IInteractalbe interactalbe)
			{
				interactalbe.Interact();
				MapChanged?.Invoke(state);
			}
		}

		public void MoveEntity(GameState state, Vector target, IOccupier entity)
		{
			
			state.Map[entity.Pos] = new(entity.Pos, null);
			state.Map[target] = new(target, entity);

			entity.Pos = target;

			MapChanged?.Invoke(state);
			
		}

		public void Interact(IOccupier actor, Vector dir, GameState state)
		{
			Vector targetPos = actor.Pos.Add(new(dir.X, -dir.Y));

			if (!state.Map.ContainsKey(targetPos)) { GD.Print("target out of bound") ;return; }

			IOccupier obj = state.Map[targetPos].Occupier;

			if(actor.OnHand != null)
			{
				if((actor.OnHand.CapablityCode & obj.AbilityCode) == 0) return;

				if (obj?.OnHand != null && obj.OnHand is  Pot con)
				{
					if(actor.OnHand is Ingredint ingredint)
					{
						con.AddIngredient(ingredint);
						// Debug.WriteLine(con.Ingredints.Count <= 0);
					}

					actor.OnHand = null;

					state.Map[targetPos] = new(targetPos, obj);
					MapChanged?.Invoke(state);

					return;
					
				}
				

				obj.OnHand = actor.OnHand;
				actor.OnHand = null;
				MapChanged?.Invoke(state);
				


				return;
			}

			if(actor.OnHand == null)
			{
				if(obj.OnHand == null) {GD.Print("nothing to take"); return; }

				actor.OnHand = obj.OnHand;
				obj.OnHand = null;

				if(obj is GoodsStock stock)
				{
					stock.Reduce();
				}

				GD.Print("took the object");
			}

			MapChanged?.Invoke(state);
		}

       

        public void ProcessTick(GameState state)
		{
			if(state.Tics >= 5) state.Tics = 0; else state.Tics++ ;


			foreach(var station in state.WorkStations)
			{
				if(station.ToolState != State.ON) return;
				station.ProgressActive();
				station.ProgressPassive();
			}
			MapChanged?.Invoke(state);
		}
	}

	public class EventErrors
	{
		
	}

	public class Varifier
	{
		public static int Coockable => 1;
		public static int Choppable => 2;
		public static int Washable  => 4;
		public static int Platable  => 8;

	}
}
