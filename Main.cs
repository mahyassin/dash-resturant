using System;
using Godot;
using Scripts.Models;

namespace Scripts.Controllers
{
	public partial class Main : Node2D
	{
		[Export] private RichTextLabel textView;
		[Export] private RichTextLabel clock;
		[Export] private InputReader _inputs;
		[Export] private Timer timer;

		[Export] private Label Containers;


		private GameState _state;
		private Systems _systems = new();
		private MapModel _map = new();
		private StateDecoder stateRender = new();
		public override void _Ready()
		{
			_inputs.Moved += OnMove;
			_inputs.InterActed += OnInterActed;
			_systems.MapChanged += UpdateMap;

			_state = _map.GetMapCodeState();

			UpdateMap(_state);

			timer.Timeout += OnTimerTick;

			test();

		}

        private void test()
        {
            Ingredint tomato = new(IngredintType.TOMATO);
			Pot pot = new();
			pot.AddIngredient(tomato);

			GD.Print(pot.GetIngredients().Count);
        }

        private	void OnMove(Vector dir)
		{
			_systems.Move(_state.Player, _state, dir);

		}
		private void OnInterActed(Vector dir)
		{
			GD.Print("interacted");
			_systems.Interact(_state.Player, dir, _state);
		}

		private void UpdateMap(GameState state)
		{
			string text = "";

			foreach(var line in stateRender.DecodeState(state)){
				text += line +"\n";
				
			}

			textView.Text = text;
			clock.Text = stateRender.DecodeClock(state);

			
		}

		private void OnTimerTick()
		{
			_systems.ProcessTick(_state);
			UpdateMap(_state);
			
		}

	}
	

}

public struct Vector{
	public readonly int X;
	public readonly int Y;
	public Vector(int x = 0, int y = 0)
	{
		X = x;
		Y = y;
	}
	public Vector Add(Vector v)
	{
		return new(X + v.X, Y + v.Y);
	}
}
