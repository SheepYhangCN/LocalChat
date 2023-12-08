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
		if (image!=null)
		{
			GetNode<Button>("Actions/Copy").Modulate=new Color(1,1,1,0);
		}
		GetNode<Label>("VBoxContainer/HBoxContainer/DebugID").Text=id.ToString();
		GetNode<Control>("Space").CustomMinimumSize=new Vector2(GetNode<Button>("../../../../../MemberList").Size.X,0);
	}

	public override void _Process(double delta)
	{
		var children=GetNode<ChatRoom>("../../../../../").GetChildren();
		var visible_test=true;
		for (var a=0;a<children.Count;a+=1)
		{
			if (children[a] is not VBoxContainer && children[a].Name!="MemberList" && (bool)children[a].Get("visible"))
			{
				visible_test=false;
			}
		}
		//visible_test=(!GetNode<IPs>("../../../../../IPs").Visible)&&(!GetNode<Button>("../../../../../List").Visible);
		GetNode<VBoxContainer>("Actions").Modulate=new Color(1,1,1,(new Rect2(GlobalPosition,Size).HasPoint(GetGlobalMousePosition()) && visible_test) ? 1 : 0);;
	}

	public void _on_copy_pressed()
	{
		if (image==null)
		{
			DisplayServer.ClipboardSet(GetNode<Label>("VBoxContainer/HBoxContainer2/Message").Text);
		}
	}

	public void _on_delete_pressed()
	{
		GetNode<ChatRoom>("../../../../../").Rpc("DeleteMessage",id);
		QueueFree();
	}
}
