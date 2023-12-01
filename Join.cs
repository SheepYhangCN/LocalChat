using Godot;
using System;

public partial class Join : Control
{
	public override void _Ready()
	{
		Multiplayer.ConnectedToServer+=connected;
		var save=new ConfigFile();
		save.Load("user://LocalChat.ini");
		GetNode<LineEdit>("VBoxContainer/HBoxContainer2/LineEdit").Text=(string)save.GetValue("User","UsedIP","");
		GetNode<LineEdit>("VBoxContainer/HBoxContainer2/LineEdit2").Text=(string)save.GetValue("User","UsedPort",1145);
	}

	public override void _Process(double delta)
	{
		if (GetNode<LineEdit>("VBoxContainer/HBoxContainer2/LineEdit").Text=="" || GetNode<LineEdit>("VBoxContainer/HBoxContainer2/LineEdit").Text==null || GetNode<LineEdit>("VBoxContainer/HBoxContainer2/LineEdit2").Text=="" || GetNode<LineEdit>("VBoxContainer/HBoxContainer2/LineEdit2").Text==null)
		{
			GetNode<Button>("VBoxContainer/HBoxContainer/Join").Disabled=true;
		}
		else
		{
			GetNode<Button>("VBoxContainer/HBoxContainer/Join").Disabled=false;
		}
	}

	public void _on_back_pressed()
	{
		GetTree().ChangeSceneToFile("res://Menu.tscn");
	}

	public void _on_join_pressed()
	{
		var ip=GetNode<LineEdit>("VBoxContainer/HBoxContainer2/LineEdit").Text;
		var port=GetNode<LineEdit>("VBoxContainer/HBoxContainer2/LineEdit2").Text.ToInt();
		SaveIP(ip,port);
		GetNode<AutoLoad>("/root/AutoLoad").ip=ip;
		GetNode<AutoLoad>("/root/AutoLoad").port=port;
		var client=new ENetMultiplayerPeer();
		var err=client.CreateClient(ip,port);
		Multiplayer.MultiplayerPeer=client;
		GD.Print(err.ToString()+" "+ip.ToString()+":"+port.ToString()+" ");
	}

	private static void SaveIP(string ip,int port)
	{
		var save=new ConfigFile();
		save.Load("user://LocalChat.ini");
		save.SetValue("User","UsedIP",ip);
		save.SetValue("User","UsedPort",port);
		save.Save("user://LocalChat.ini");
	}

	private async void connected()
	{
		await ToSignal(GetTree().CreateTimer(1f),"timeout");
		GetTree().ChangeSceneToFile("res://ChatRoom/ChatRoom.tscn");
	}
}
