using Godot;
using System;
using System.Collections.Generic;

namespace Scripts.Models
{
	public class GameState
	{
		private Dictionary<Vector, CellState> _map;
		public int MapWidth {get;}
		public int Maphieght {get;}
		private Player _player;
		public List<IInteractalbe> WorkStations { get;}

		public int Tics = 0;
	
		public GameState(int mapWidth, int maphieght, Player player, Dictionary<Vector, CellState> map, List<IInteractalbe> tools)
		{
			Maphieght = maphieght;
			MapWidth = mapWidth;
			_player = player;
			_map = new(map);
			WorkStations = tools;

		}

		public Dictionary<Vector, CellState> Map => _map;
		public Player Player => _player;
	}


public class CellState{
		public Vector Pos;
		public bool IsWalkable => Occupier == null;
		public IOccupier Occupier = null;
		
		public CellState(Vector pos, IOccupier occupier)
		{
			Pos = pos;
			Occupier = occupier;
		}
	}

}
