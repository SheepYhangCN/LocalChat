using Godot;
using System;

public partial class Message : HBoxContainer
{
	internal int id=0;
	internal int peer=1;
	internal string name="";
	internal string time="";
	internal string message="";
	internal Texture2D image=null;
	public override void _Ready()
	{
		GetNode<Label>("VBoxContainer/HBoxContainer/Name").Text=name;
		GetNode<Label>("VBoxContainer/HBoxContainer/ID").Text="("+peer.ToString()+")";
		GetNode<Label>("VBoxContainer/HBoxContainer/Time").Text="["+time+"]";
		GetNode<Label>("VBoxContainer/HBoxContainer2/Message").Text=message;
		GetNode<Sprite2D>("VBoxContainer/HBoxContainer2/Sprite2D").Texture=image;
		if (Multiplayer.IsServer() || peer==Multiplayer.MultiplayerPeer.GetUniqueId())
		{
			GetNode<Button>("Actions/Delete").Modulate=new Color(1,1,1,1);
		}
		GetNode<Label>("VBoxContainer/HBoxContainer/DebugID").Text=id.ToString();
	}

	public override void _Process(double delta)
	{
		GetNode<VBoxContainer>("Actions").Modulate=new Color(1,1,1,(new Rect2(GlobalPosition,Size).HasPoint(GetGlobalMousePosition()) && !GetNode<IPs>("../../../../../IPs").Visible && !GetNode<Button>("../../../../../List").Visible) ? 1 : 0);;
	}

	public void _on_copy_pressed()
	{
		DisplayServer.ClipboardSet(GetNode<Label>("VBoxContainer/HBoxContainer2/Message").Text);
	}

	public void _on_delete_pressed()
	{
		GetNode<ChatRoom>("../../../../../").Rpc("DeleteMessage",id);
		QueueFree();
	}
}
