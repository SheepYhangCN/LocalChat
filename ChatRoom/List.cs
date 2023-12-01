using Godot;
using System;

public partial class List : Button
{
	PackedScene member=GD.Load<PackedScene>("res://ChatRoom/Member.tscn");
	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
	}

	internal void Update()
	{
		var children=GetNode<VBoxContainer>("Panel/ScrollContainer/VBoxContainer").GetChildren();
		for (var a=0;a+1<children.Count;a+=1)
		{
			children[a+1].QueueFree();
		}
		var autoload=GetNode<AutoLoad>("/root/AutoLoad");
		var array=Multiplayer.GetPeers();
		var count=array.Length;
		var ins=member.Instantiate<Member>();
		ins.name=autoload.name;
		ins.id=Multiplayer.GetUniqueId();
		GetNode<VBoxContainer>("Panel/ScrollContainer/VBoxContainer").AddChild(ins);
		for (var a=0;a<count;a+=1)
		{
			var inst=member.Instantiate<Member>();
			GetNode<ChatRoom>("../").RpcId(array[a],"GetInfo",Multiplayer.GetUniqueId());
			inst.name=(string)((Godot.Collections.Array)autoload.received)[1];
			inst.id=(int)((Godot.Collections.Array)autoload.received)[0];
			GetNode<VBoxContainer>("Panel/ScrollContainer/VBoxContainer").AddChild(inst);
		}
	}
}
