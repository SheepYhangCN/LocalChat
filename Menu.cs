using Godot;
using System;

public partial class Menu : Control
{
	public override void _Ready()
	{
		var node=GetNode<OptionButton>("OptionButton");
		var autoload=GetNode<AutoLoad>("/root/AutoLoad");
		switch (TranslationServer.GetLocale())
		{
			case "en":
				node.Selected=0;
				break;
			case "zh_CN":
				node.Selected=1;
				break;
			case "zh_TW":
				node.Selected=2;
				break;
			case "ja":
				node.Selected=3;
				break;
		}
		GetNode<Label>("Version").Text=autoload.version;
		if (autoload.popup >= 1)
		{
			if (autoload.popup == 1)
			{
				if (autoload.is_connection_lost)
				{
					GetNode<Label>("Popup/PanelContainer/Label").Text="locConnectionLost";
				}
				else
				{
					GetNode<Label>("Popup/PanelContainer/Label").Text="locYouWasRemoved";
				}
			}
			else if (autoload.popup == 2)
			{
				GetNode<Label>("Popup/PanelContainer/Label").Text="locSha256DoesntMatch";
			}
			else if (autoload.popup == 3)
			{
				GetNode<Label>("Popup/PanelContainer/Label").Text="locVersionDoesntMatch";
			}
			autoload.popup=0;
			autoload.is_connection_lost=false;
			GetNode<Panel>("Popup").Visible=true;
		}
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

	public void _on_option_button_item_selected(int selected)
	{
		switch (selected)
		{
			case 0:
				TranslationServer.SetLocale("en");
				break;
			case 1:
				TranslationServer.SetLocale("zh_CN");
				break;
			case 2:
				TranslationServer.SetLocale("zh_TW");
				break;
			case 3:
				TranslationServer.SetLocale("ja");
				break;
		}
		GetNode<ip>("VBoxContainer/ip")._Ready();
		var save=new ConfigFile();
		save.Load("user://LocalChat.ini");
		save.SetValue("Settings","Language",TranslationServer.GetLocale());
		save.Save("user://LocalChat.ini");
	}

	public void _on_ok_pressed()
	{
		GetNode<Panel>("Popup").Visible=false;
	}

	private static void SaveName(string name)
	{
		var save=new ConfigFile();
		save.Load("user://LocalChat.ini");
		save.SetValue("User","Name",name);
		save.Save("user://LocalChat.ini");
	}
}
