using Godot;
using System;
using Microsoft.Toolkit.Uwp.Notifications;

public partial class AutoLoad : Node
{
	internal string name="";
	internal string ip="127.0.0.1";
	internal int port=1145;
	internal int popup=0;
	internal bool is_connection_lost=false;
	internal string version="v2024.3.7";
	internal bool notification=true;
	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
	}

	public override void _Notification(int what)
	{
	    if (OS.GetName() == "Windows" && what == NotificationWMCloseRequest)
		{
			ToastNotificationManagerCompat.History.Clear();
			ToastNotificationManagerCompat.Uninstall();
		}
	}
}
