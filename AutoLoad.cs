using Godot;
using System;
#if WINDOWS
using Microsoft.Toolkit.Uwp.Notifications;
#endif

public partial class AutoLoad : Node
{
	internal string name="";
	internal string ip="127.0.0.1";
	internal int port=1145;
	internal int popup=0;
	internal bool is_connection_lost=false;
	internal string version="v"+ProjectSettings.GetSetting("application/config/version");
	internal bool notification=true;
	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
	}

	public override void _Notification(int what)
	{
	    if (what == NotificationWMCloseRequest)
		{
			#if WINDOWS
			ToastNotificationManagerCompat.History.Clear();
			ToastNotificationManagerCompat.Uninstall();
			#endif
		}
	}
}
