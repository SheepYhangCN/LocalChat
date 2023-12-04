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
						Text=Text.TrimSuffix("\n");
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
