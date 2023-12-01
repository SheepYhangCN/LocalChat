using Godot;
using System;

public partial class LineEdit1 : Godot.LineEdit
{
	public override void _Ready()
	{
		var save=new ConfigFile();
		save.Load("user://LocalChat.ini");
		Text=(string)save.GetValue("User","Name","");
		GetNode<AutoLoad>("/root/AutoLoad").name=Text;
	}

	public override void _Process(double delta)
	{
	}
}
