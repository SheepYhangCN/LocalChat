using Godot;
using System;

public partial class AutoLoad : Node
{
	internal string name="";
	internal string ip="127.0.0.1";
	internal int port=1145;
	internal bool removed=false;
	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
	}
}
