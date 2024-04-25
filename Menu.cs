using Godot;
using System;
using Microsoft.Toolkit.Uwp.Notifications;

public partial class Menu : Control
{
	public override void _Ready()
	{
		var autoload=GetNode<AutoLoad>("/root/AutoLoad");
		var node=GetNode<OptionButton>("OptionButton");
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
		GetNode<CheckButton>("HBoxContainer/Control2/CheckButton").ButtonPressed=autoload.notification;
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
			if (autoload.notification && !GetWindow().HasFocus())
			{
				if (OS.GetName() == "Windows")
				{
					var timed=Time.GetDatetimeDictFromSystem(false);
					new ToastContentBuilder()
						.AddArgument("action", "viewConversation")
   						.AddArgument("conversationId", 9813)
						.AddText(TranslationServer.Translate(autoload.popup == 1 ? "locBackedToMenu" : "locConnectFailed"))
						.AddText(TranslationServer.Translate(GetNode<Label>("Popup/PanelContainer/Label").Text))
						.AddCustomTimeStamp(new DateTime((int)timed["year"],(int)timed["month"],(int)timed["day"],(int)timed["hour"],(int)timed["minute"],(int)timed["second"],DateTimeKind.Local))
						.Show();
				}
				if (OS.GetName() == "Linux")
				{
					OS.Execute("bash",["notify-send",TranslationServer.Translate(autoload.popup == 1 ? "locBackedToMenu" : "locConnectFailed"),TranslationServer.Translate(GetNode<Label>("Popup/PanelContainer/Label").Text)]);
				}
			}
			autoload.popup=0;
			autoload.is_connection_lost=false;
			GetNode<Panel>("Popup").Visible=true;
		}
		ContainerUpdate(GetNode<VBoxContainer>("VBoxContainer"));
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
		GetNode<ip>("VBoxContainer/ScrollContainer/PanelContainer/ip")._Ready();
		var save=new ConfigFile();
		save.Load("user://LocalChat.ini");
		save.SetValue("Settings","Language",TranslationServer.GetLocale());
		save.Save("user://LocalChat.ini");
		ContainerUpdate(GetNode<VBoxContainer>("VBoxContainer"));
	}

	public void _on_check_button_toggled(bool a)
	{
		GetNode<AutoLoad>("/root/AutoLoad").notification=a;
		var save=new ConfigFile();
		save.Load("user://LocalChat.ini");
		save.SetValue("Settings","Notification",a);
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

	private static void ContainerUpdate(VBoxContainer n)
	{
		n.Size=new Vector2(n.GetCombinedMinimumSize().X,n.Size.Y);
		n.Position=new Vector2(640-n.Size.X/2,n.Position.Y);
	}
}
