using Godot;
using System;
using Scripts.Controllers;
using Microsoft.VisualBasic;

public partial class InputReader : Node
{

    private Button[] _buttons =
    {
        new Button(new(0, 1), "Moving_Up"),
        new Button(new(0, -1), "Moving_Down"),
        new Button(new(1, 0), "Moving_Right"),
        new Button(new(-1, 0), "Moving_Left"),
    };

    
    public event Action<Vector> Moved;
    public event Action<Vector> InterActed;
    private bool holdLock = false;



    public override void _Input(InputEvent @event)
    {

        int x = 0;
        int y = 0;

        
        foreach(var button in _buttons)
        {
            if(@event.IsActionReleased(button.Name))
            {
                if(holdLock){holdLock = false; return;}

                x+= button.Dir.X;
                y+= button.Dir.Y;

                GD.Print($"{x}, {y}");
                Moved?.Invoke(new(x, y));

                holdLock = false;

            } 

            if (!@event.IsActionPressed(button.Name, true))  continue;

            x+= button.Dir.X;
            y+= button.Dir.Y;

            if(holdLock) return;
            
            if(@event.IsEcho())
            {
                holdLock = true;
                InterActed.Invoke(new(x, y));

                GD.Print($" holding {x}, {y}");
            }
            // GD.Print($"{x}, {y}");
            
            // Moved?.Invoke(new(x, y));

        }
        
        
    }

    public struct Button
    {
        public Vector Dir;
        public string Name;

        public Button(Vector dir, string name)
        {
            Dir = dir;
            Name = name;
        }
    }

  
}
