using Godot;
using Godot.Collections;
using System;

public partial class ChatRoom : Control
{
	PackedScene message_packed=GD.Load<PackedScene>("res://ChatRoom/Message.tscn");
	PackedScene sys_message_packed=GD.Load<PackedScene>("res://ChatRoom/SystemMessage.tscn");
	public override void _Ready()
	{
		Multiplayer.ServerDisconnected+=disconnected;
		Multiplayer.MultiplayerPeer.SetTargetPeer((int)MultiplayerPeer.TargetPeerBroadcast);
		GetNode<Label>("VBoxContainer/Title/Panel/Label").Text=TranslationServer.Translate("locCurrentUsername")+GetNode<AutoLoad>("/root/AutoLoad").name+" ("+Multiplayer.GetUniqueId().ToString()+")";
		if (Multiplayer.IsServer())
		{
			GetNode<Button>("VBoxContainer/Title/Quit").Text=TranslationServer.Translate("locCloseChatroom");
		}
		Rpc("SendSystemMessage",GetNode<AutoLoad>("/root/AutoLoad").name+TranslationServer.Translate("locJoined"));
		SendSystemMessage(GetNode<AutoLoad>("/root/AutoLoad").name+TranslationServer.Translate("locJoined"));
	}

	public override void _Process(double delta)
	{
	}

	public async void _on_quit_pressed()
	{
		GetNode<Button>("VBoxContainer/Title/Quit").Disabled=true;
		Rpc("SendSystemMessage",GetNode<AutoLoad>("/root/AutoLoad").name+TranslationServer.Translate("locQuitted"));
		SendSystemMessage(GetNode<AutoLoad>("/root/AutoLoad").name+TranslationServer.Translate("locQuitted"));
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
		ins.peer=peer;
		ins.name=name;
		ins.time=time;
		ins.message=message;
		GetNode<VBoxContainer>("VBoxContainer/Panel/ScrollContainer/VBoxContainer").AddChild(ins);
	}
	[Rpc(MultiplayerApi.RpcMode.AnyPeer)]
	internal void SendSystemMessage(string message)
	{
		GetNode<List>("List").Update();
		var ins=sys_message_packed.Instantiate<HBoxContainer>();
		ins.GetNode<Label>("PanelContainer/Message").Text="["+Time.GetDatetimeStringFromSystem(false,true)+"]\n"+message;
		GetNode<VBoxContainer>("VBoxContainer/Panel/ScrollContainer/VBoxContainer").AddChild(ins);
	}
	[Rpc(MultiplayerApi.RpcMode.AnyPeer)]
	internal void GetInfo(int from)
	{
		RpcId(from,"Receive",(Variant)new Godot.Collections.Array(){Multiplayer.MultiplayerPeer.GetUniqueId(),GetNode<AutoLoad>("/root/AutoLoad").name});
	}
	[Rpc(MultiplayerApi.RpcMode.AnyPeer)]
	internal void Receive(Variant anything)
	{
		GetNode<AutoLoad>("/root/AutoLoad").received=anything;
	}
}
