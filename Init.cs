using Godot;
using System;

public partial class Init : Node2D
{
	public override void _Ready()
	{
		if (OS.GetLocale() == "zh_TW" || OS.GetLocale() == "zh_HK" || OS.GetLocale() == "zh_MO")
		{
		    TranslationServer.SetLocale("zh_TW");
		}
		else if (OS.GetLocaleLanguage() == "zh" || OS.GetLocale() == "zh_CN" || OS.GetLocale() == "zh_SG")
		{
		    TranslationServer.SetLocale("zh_CN");
		}
		else
		{
		    TranslationServer.SetLocale(OS.GetLocale());
		}
		if (FileAccess.FileExists("user://LocalChat.ini"))
		{
			var save=new ConfigFile();
			save.Load("user://LocalChat.ini");
			TranslationServer.SetLocale((string)save.GetValue("Settings","Language","en"));
			GetNode<AutoLoad>("/root/AutoLoad").notification=(bool)save.GetValue("Settings","Notification",true);
		}
		else
		{
			var save=new ConfigFile();
			save.SetValue("Settings","Language",TranslationServer.GetLocale());
			save.Save("user://LocalChat.ini");
		}
		GetTree().ChangeSceneToFile("res://Menu.tscn");
	}

	public override void _Process(double delta)
	{
	}
}
