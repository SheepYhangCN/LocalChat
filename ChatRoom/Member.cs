using Godot;
using System;

public partial class Member : HBoxContainer
{
	internal string name="";
	internal int id=1;
	public override void _Ready()
	{
		GetNode<Label>("Name").Text=name;
		GetNode<Label>("ID").Text=" ("+id.ToString()+")";
	}

	public override void _Process(double delta)
	{
	}
}
