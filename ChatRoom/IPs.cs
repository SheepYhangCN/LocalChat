using Godot;
using System;

public partial class IPs : Button
{
	public override void _Ready()
	{
		GetNode<Label>("Panel/ScrollContainer/VBoxContainer/Port").Text=TranslationServer.Translate("locPortColon")+GetNode<AutoLoad>("/root/AutoLoad").port.ToString();
		var scene=GD.Load<PackedScene>("res://ChatRoom/IPs_IP.tscn");
		if (Multiplayer.IsServer())
		{
			var ips=Menu.GetIPs();
			for (var a=0;a<ips.Count;a+=1)
			{
				var inst=scene.Instantiate<Label>();
				GetNode<VBoxContainer>("Panel/ScrollContainer/VBoxContainer").AddChild(inst);
				inst.Text=ips[a];
			}
		}
		else
		{
			var inst=scene.Instantiate<Label>();
			GetNode<VBoxContainer>("Panel/ScrollContainer/VBoxContainer").AddChild(inst);
			inst.Text=GetNode<AutoLoad>("/root/AutoLoad").ip;
		}
	}

	public override void _Process(double delta)
	{
	}
}
