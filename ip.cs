using Godot;
using System;

public partial class ip : Label
{
	public override void _Ready()
	{
		var ips=Menu.GetIPs();
		var ip="";
		for (var a=0;a<ips.Count;a+=1)
		{
			ip+="\n"+ips[a];
		}
		Text="【你的IP】"+ip+"\n";
	}

	public override void _Process(double delta)
	{
	}
}
