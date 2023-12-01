using Godot;
using System;

public partial class Menu : Control
{
	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
		if (GetNode<LineEdit>("VBoxContainer/LineEdit1").Text=="" || GetNode<LineEdit>("VBoxContainer/LineEdit1").Text==null)
		{
			GetNode<Button>("VBoxContainer/HBoxContainer/Create").Disabled=true;
			GetNode<Button>("VBoxContainer/HBoxContainer/Join").Disabled=true;
		}
		else
		{
			GetNode<Button>("VBoxContainer/HBoxContainer/Create").Disabled=false;
			GetNode<Button>("VBoxContainer/HBoxContainer/Join").Disabled=false;
		}
	}

	internal static Godot.Collections.Array<string> GetIPs()
	{
		var ips=IP.GetLocalAddresses();
		var ip=new Godot.Collections.Array<string>();
		for (var a=0;a<ips.Length;a+=1)
		{
			if (ips[a]!="0:0:0:0:0:0:0:1"&&ips[a]!="127.0.0.1"&&ips[a]!="localhost"&&ips[a]!="0.0.0.0"&&ips[a]!="255.255.255.255")
			{
				ip.Add(ips[a]);
			}
		}
		return ip;
	}

	public void _on_create_pressed()
	{
		GetNode<AutoLoad>("/root/AutoLoad").name=GetNode<LineEdit>("VBoxContainer/LineEdit1").Text;
		SaveName(GetNode<LineEdit>("VBoxContainer/LineEdit1").Text);
		GetTree().ChangeSceneToFile("res://Create.tscn");
	}

	public void _on_join_pressed()
	{
		GetNode<AutoLoad>("/root/AutoLoad").name=GetNode<LineEdit>("VBoxContainer/LineEdit1").Text;
		SaveName(GetNode<LineEdit>("VBoxContainer/LineEdit1").Text);
		GetTree().ChangeSceneToFile("res://Join.tscn");
	}

	private static void SaveName(string name)
	{
		var save=new ConfigFile();
		save.Load("user://LocalChat.ini");
		save.SetValue("User","Name",name);
		save.Save("user://LocalChat.ini");
	}
}
