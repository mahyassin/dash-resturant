using Godot;
using System;

public partial class MapView : Node
{
    [Export] private RichTextLabel mapDispaly;
	[Export] private Label clockDisplay;


    public void DrawMap(string[] mapCode)
    {
        mapDispaly.Text = "";
        foreach(var row in mapCode)
        {
            mapDispaly.Text += row;
        }
    }


}
