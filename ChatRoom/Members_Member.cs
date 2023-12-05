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
			GetNode<Button>("Actions/Remove").Visible=true;
		}
	}

	public override void _Process(double delta)
	{
		if (peer!=Multiplayer.MultiplayerPeer.GetUniqueId())
		{
			GetNode<VBoxContainer>("Actions").Visible=new Rect2(GlobalPosition,Size).HasPoint(GetGlobalMousePosition());
		}
	}

	public void _on_remove_pressed()
	{
		GetNode<ChatRoom>("../../../../../").RpcId(peer,"Removed");
	}
}
