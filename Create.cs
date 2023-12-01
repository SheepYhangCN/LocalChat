using Godot;
using System;

public partial class Create : Control
{
	public override void _Ready()
	{
		var save=new ConfigFile();
		save.Load("user://LocalChat.ini");
		GetNode<LineEdit>("VBoxContainer/HBoxContainer2/LineEdit").Text=(string)save.GetValue("Host","Port",1145);
	}

	public override void _Process(double delta)
	{
		if (GetNode<LineEdit>("VBoxContainer/HBoxContainer2/LineEdit").Text=="" || GetNode<LineEdit>("VBoxContainer/HBoxContainer2/LineEdit").Text==null)
		{
			GetNode<Button>("VBoxContainer/HBoxContainer/Create").Disabled=true;
		}
		else
		{
			GetNode<Button>("VBoxContainer/HBoxContainer/Create").Disabled=false;
		}
	}

	public void _on_back_pressed()
	{
		GetTree().ChangeSceneToFile("res://Menu.tscn");
	}

	public void _on_create_pressed()
	{
		var port=GetNode<LineEdit>("VBoxContainer/HBoxContainer2/LineEdit").Text.ToInt();
		SavePort(port);
		GetNode<AutoLoad>("/root/AutoLoad").port=port;
		var server=new ENetMultiplayerPeer();
		var err=server.CreateServer(port);
		Multiplayer.MultiplayerPeer=server;
		GetTree().ChangeSceneToFile("res://ChatRoom/ChatRoom.tscn");
		GD.Print(err.ToString()+" "+port.ToString()+" ");
	}

	private static void SavePort(int ip)
	{
		var save=new ConfigFile();
		save.Load("user://LocalChat.ini");
		save.SetValue("Host","Port",ip);
		save.Save("user://LocalChat.ini");
	}
}
