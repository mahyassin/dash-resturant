using Godot;
using System;
using System.Diagnostics;

namespace Scripts.Models
{

	public partial class Systems
	{
		public event Action<GameState> MapChanged;

		public void Move(IOccupier entity, GameState state, Vector dir )
		{
			Vector target = entity.Pos.Add(new(dir.X, -dir.Y));
			if (!state.Map.ContainsKey(target)) { GD.Print("target out of bound") ;return; }

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
			Vector target = actor.Pos.Add(new(dir.X, -dir.Y));

			if (!state.Map.ContainsKey(target)) { GD.Print("target out of bound") ;return; }

			IOccupier obj = state.Map[target].Occupier;

			if(obj is null) 
			{
				GD.Print("there is nothing here"); return ;
			
			}

			if(actor.OnHand != null)
			{
				if(obj.OnHand != null) {GD.Print("no space"); return; }

				obj.OnHand = actor.OnHand;
				actor.OnHand = null;
				GD.Print("put the object");

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
				if(station.StationState != WorkingStation.State.ON) 
				{
					continue;
				} 
				
				if(station.progressType == ProgressType.PASSIVE)
				{
					if(station.OnHand == null) return;

					station.OnHand.Progress[station.progressType]++;
					continue;

				};

				if(station.progressType == ProgressType.ACTIVE)
				{
					if(station.OnHand == null) return;


					station.OnHand.Progress[station.progressType]++;
					station.Interact();
					continue;

				};

			}
		}
	}
}
