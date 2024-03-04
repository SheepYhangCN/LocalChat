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
		if (Multiplayer.IsServer())
		{
			GetNode<Button>("Actions/Remove").Modulate=new Color(1,1,1,1);
		}
	}

	public override void _Process(double delta)
	{
		if (peer!=Multiplayer.MultiplayerPeer.GetUniqueId())
		{
			GetNode<VBoxContainer>("Actions").Modulate=new Color(1,1,1,new Rect2(GlobalPosition,Size).HasPoint(GetGlobalMousePosition()) ? 1 : 0);
		}
	}

	public void _on_remove_pressed()
	{
		GetNode<ChatRoom>("../../../../../").RpcId(peer,"Removed");
	}

	public void _on_ping_pressed()
	{
		GetNode<ChatRoom>("../../../../../").RpcId(peer,"Pinged",Multiplayer.MultiplayerPeer.GetUniqueId(),GetNode<AutoLoad>("/root/AutoLoad").name);
	}
}
