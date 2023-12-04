using Godot;
using System;

public partial class Message : VBoxContainer
{
	internal int peer=1;
	internal string name="";
	internal string time="";
	internal string message="";
	public override void _Ready()
	{
		GetNode<Label>("HBoxContainer/Name").Text=name;
		GetNode<Label>("HBoxContainer/ID").Text="("+peer.ToString()+")";
		GetNode<Label>("HBoxContainer/Time").Text="["+time+"]";
		GetNode<Label>("HBoxContainer2/Message").Text=message;
	}

	public override void _Process(double delta)
	{
	}
}