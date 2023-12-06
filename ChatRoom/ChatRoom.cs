using Godot;
using Godot.Collections;
using System;

public partial class ChatRoom : Control
{
	PackedScene message_packed=GD.Load<PackedScene>("res://ChatRoom/Message.tscn");
	PackedScene sys_message_packed=GD.Load<PackedScene>("res://ChatRoom/SystemMessage.tscn");
	PackedScene member=GD.Load<PackedScene>("res://ChatRoom/Members_Member.tscn");
	internal int message_id_next=0;
	public override void _Ready()
	{
		Multiplayer.ServerDisconnected+=disconnected;
		Multiplayer.MultiplayerPeer.SetTargetPeer((int)MultiplayerPeer.TargetPeerBroadcast);
		GetNode<Label>("VBoxContainer/Title/Panel/Label").Text=TranslationServer.Translate("locCurrentUsername")+GetNode<AutoLoad>("/root/AutoLoad").name+" ("+Multiplayer.GetUniqueId().ToString()+")";
		if (Multiplayer.IsServer())
		{
			GetNode<Button>("VBoxContainer/Title/Quit").Text=TranslationServer.Translate("locCloseChatroom");
			Joined(GetNode<AutoLoad>("/root/AutoLoad").name,Multiplayer.MultiplayerPeer.GetUniqueId());
		}
		else
		{
			Rpc("Joined",GetNode<AutoLoad>("/root/AutoLoad").name,Multiplayer.MultiplayerPeer.GetUniqueId());
			RpcId(MultiplayerPeer.TargetPeerServer,"SyncFromServer",Multiplayer.MultiplayerPeer.GetUniqueId());
		}
		SendSystemMessage(GetNode<AutoLoad>("/root/AutoLoad").name+TranslationServer.Translate("locJoined"));
		Rpc("SendSystemMessage",GetNode<AutoLoad>("/root/AutoLoad").name+TranslationServer.Translate("locJoined"));
	}

	public override void _Process(double delta)
	{
	}

	public async void _on_quit_pressed()
	{
		GetNode<Button>("VBoxContainer/Title/Quit").Disabled=true;
		Rpc("Quitted",Multiplayer.MultiplayerPeer.GetUniqueId());
		Quitted(Multiplayer.MultiplayerPeer.GetUniqueId());
		SendSystemMessage(GetNode<AutoLoad>("/root/AutoLoad").name+TranslationServer.Translate("locQuitted"));
		Rpc("SendSystemMessage",GetNode<AutoLoad>("/root/AutoLoad").name+TranslationServer.Translate("locQuitted"));
		await ToSignal(GetTree().CreateTimer(0.25), "timeout");
		if (Multiplayer.IsServer()){
			var peers=Multiplayer.GetPeers();
			for (var a=0;a<peers.Length;a+=1)
			{
				Multiplayer.MultiplayerPeer.DisconnectPeer(peers[a]);
			}
		}
		Multiplayer.MultiplayerPeer.Close();
		Multiplayer.MultiplayerPeer=null;
		GetTree().ChangeSceneToFile("res://Menu.tscn");
	}

	public void _on_i_ps_pressed()
	{
		GetNode<Button>("IPs").Visible=true;
	}

	public void _on_i_ps_b_pressed()
	{
		GetNode<Button>("IPs").Visible=false;
	}

	public void _on_member_list_pressed()
	{
		GetNode<Button>("List").Visible=true;
	}

	public void _on_list_pressed()
	{
		GetNode<Button>("List").Visible=false;
	}

	public void _on_send_pressed()
	{
		if (GetNode<TextEdit>("VBoxContainer/Input/Text").Text!="")
		{
			var text=GetNode<TextEdit>("VBoxContainer/Input/Text").Text;
			text=text.Replace("\n\n","\n \n").Replace("\n\n","\n \n");
			if (text.StartsWith("\n"))
			{
				text=" \n"+text.TrimPrefix("\n");
			}
			else if (text.EndsWith("\n"))
			{
				text=text.TrimSuffix("\n")+"\n ";
			}
			SendMessage(Multiplayer.MultiplayerPeer.GetUniqueId(),GetNode<AutoLoad>("/root/AutoLoad").name,Time.GetDatetimeStringFromSystem(false,true),text);
			Rpc("SendMessage",Multiplayer.MultiplayerPeer.GetUniqueId(),GetNode<AutoLoad>("/root/AutoLoad").name,Time.GetDatetimeStringFromSystem(false,true),text);
			GetNode<TextEdit>("VBoxContainer/Input/Text").Text="";
		}
	}

	private void disconnected()
	{
		Multiplayer.MultiplayerPeer.Close();
		Multiplayer.MultiplayerPeer=null;
		GetTree().ChangeSceneToFile("res://Menu.tscn");
	}
	[Rpc(MultiplayerApi.RpcMode.AnyPeer)]
	internal void SendMessage(int peer,string name,string time,string message)
	{
		GetNode<AudioStreamPlayer>("AudioStreamPlayer").Play();
		GD.Print(name+"("+peer.ToString()+")"+": "+message);
		var ins=message_packed.Instantiate<Message>();
		ins.id=message_id_next;
		message_id_next+=1;
		ins.peer=peer;
		ins.name=name;
		ins.time=time;
		ins.message=message;
		GetNode<VBoxContainer>("VBoxContainer/Panel/ScrollContainer/VBoxContainer").AddChild(ins);
	}
	[Rpc(MultiplayerApi.RpcMode.AnyPeer)]
	internal void SendImage(int peer,string name,string time,Texture2D image)
	{
		GetNode<AudioStreamPlayer>("AudioStreamPlayer").Play();
		GD.Print("[Image]"+name+"("+peer.ToString()+")");
		var ins=message_packed.Instantiate<Message>();
		ins.id=message_id_next;
		message_id_next+=1;
		ins.peer=peer;
		ins.name=name;
		ins.time=time;
		ins.image=image;
		GetNode<VBoxContainer>("VBoxContainer/Panel/ScrollContainer/VBoxContainer").AddChild(ins);
	}
	[Rpc(MultiplayerApi.RpcMode.AnyPeer)]
	internal void SendSystemMessage(string message)
	{
		//GetNode<List>("List").Update();
		var ins=sys_message_packed.Instantiate<HBoxContainer>();
		/*ins.id=message_id_next;
		message_id_next+=1;*/
		ins.GetNode<Label>("PanelContainer/Message").Text="["+Time.GetDatetimeStringFromSystem(false,true)+"]\n"+message;
		GetNode<VBoxContainer>("VBoxContainer/Panel/ScrollContainer/VBoxContainer").AddChild(ins);
	}
	[Rpc(MultiplayerApi.RpcMode.AnyPeer)]
	internal void Joined(string name,int peer)
	{
		var ins=member.Instantiate<Members_Member>();
		ins.name=name;
		ins.peer=peer;
		GetNode<VBoxContainer>("List/Panel/ScrollContainer/VBoxContainer").AddChild(ins);
	}
	[Rpc(MultiplayerApi.RpcMode.AnyPeer)]
	internal void Quitted(int peer)
	{
		var children=GetNode<VBoxContainer>("List/Panel/ScrollContainer/VBoxContainer").GetChildren();
		for (var a=0;a<children.Count;a+=1)
		{
			if (children[a] is Members_Member && (int)children[a].Get("peer")==peer)
			{
				children[a].QueueFree();
			}
		}
	}
	[Rpc(MultiplayerApi.RpcMode.AnyPeer)]
	internal void SyncFromServer(int peer)
	{
		var children=GetNode<VBoxContainer>("List/Panel/ScrollContainer/VBoxContainer").GetChildren();
		children.RemoveAt(0);
		for (var a=0;a<children.Count;a+=1)
		{
			RpcId(peer,"Joined",children[a].Get("name"),children[a].Get("peer"));
		}
		RpcId(peer,"SetNextMessageId",message_id_next);
	}
	[Rpc(MultiplayerApi.RpcMode.AnyPeer)]
	internal async void Removed()
	{
		GetNode<Button>("VBoxContainer/Title/Quit").Disabled=true;
		Rpc("Quitted",Multiplayer.MultiplayerPeer.GetUniqueId());
		Quitted(Multiplayer.MultiplayerPeer.GetUniqueId());
		SendSystemMessage(GetNode<AutoLoad>("/root/AutoLoad").name+TranslationServer.Translate("locWasRemoved"));
		Rpc("SendSystemMessage",GetNode<AutoLoad>("/root/AutoLoad").name+TranslationServer.Translate("locWasRemoved"));
		await ToSignal(GetTree().CreateTimer(0.25), "timeout");
		Multiplayer.MultiplayerPeer.Close();
		Multiplayer.MultiplayerPeer=null;
		GetNode<AutoLoad>("/root/AutoLoad").removed=true;
		GetTree().ChangeSceneToFile("res://Menu.tscn");
	}
	[Rpc(MultiplayerApi.RpcMode.AnyPeer)]
	internal void DeleteMessage(int id)
	{
		var children=GetNode<VBoxContainer>("VBoxContainer/Panel/ScrollContainer/VBoxContainer").GetChildren();
		for (var a=0;a<children.Count;a+=1)
		{
			if ((int)children[a].Get("id")==id)
			{
				children[a].QueueFree();
			}
		}
	}
	[Rpc(MultiplayerApi.RpcMode.AnyPeer)]
	internal void SetNextMessageId(int id)
	{
		message_id_next=id;
	}
}
