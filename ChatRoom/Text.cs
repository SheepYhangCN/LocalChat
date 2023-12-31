using Godot;
using System;

public partial class Text : TextEdit
{
	bool enterable=true;
	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
	}

    public override void _Input(InputEvent @event)
    {
		if (@event is InputEventKey && @event.IsReleased())
		{
			/*if (DisplayServer.ClipboardHasImage()&&(bool)@event.Get("ctrl_pressed")&&!(bool)@event.Get("shift_pressed")&&!(bool)@event.Get("alt_pressed")&&!(bool)@event.Get("meta_pressed")&&(Key)(long)@event.Get("keycode")==Key.V)
			{
				var texture=ImageTexture.CreateFromImage(DisplayServer.ClipboardGetImage());
				GetNode<ChatRoom>("../../../").SendImage(Multiplayer.MultiplayerPeer.GetUniqueId(),GetNode<AutoLoad>("/root/AutoLoad").name,Time.GetDatetimeStringFromSystem(false,true),texture);
				GetNode<ChatRoom>("../../../").Rpc("SendImage",Multiplayer.MultiplayerPeer.GetUniqueId(),GetNode<AutoLoad>("/root/AutoLoad").name,Time.GetDatetimeStringFromSystem(false,true),texture);
			}*/
			if ((bool)@event.Get("shift_pressed"))
			{
				if ((Key)(long)@event.Get("keycode")==Key.Enter||(Key)(long)@event.Get("keycode")==Key.KpEnter)
				{
					if (enterable)
					{
						InsertTextAtCaret("\n");
						enterable=false;
					}
				}
				else
				{
					enterable=true;
				}
			}
			else
			{
				if ((Key)(long)@event.Get("keycode")==Key.Enter||(Key)(long)@event.Get("keycode")==Key.KpEnter)
				{
					if (enterable)
					{
						Backspace();
						GetNode<ChatRoom>("../../../")._on_send_pressed();
					}
				}
				else
				{
					enterable=true;
				}
			}
		}
    }
}
