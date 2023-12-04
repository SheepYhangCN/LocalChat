using Godot;
using System;

public partial class Members_Member : HBoxContainer
{
	internal int peer=1;
	internal string name="";
	public override void _Ready()
	{
		GetNode<Label>("Name").Text=name;
		GetNode<Label>("ID").Text="("+peer.ToString()+")";
	}

	public override void _Process(double delta)
	{
	}
}
