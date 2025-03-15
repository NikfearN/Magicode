using Godot;
using System;
using DialogueManagerRuntime;

public partial class Interactable : Area2D
{
	[Export]
	public string InteractName { get; set; } = "";

	[Export]
	public bool IsInteractable { get; set; } = true;

	[Export]
	public Resource DialR { get; set; }

	[Export]
	public string DialStart { get; set; }

	private Action _interactCallback;

	public void SetInteract(Action interactCallback)
	{
		_interactCallback = interactCallback;
	}


	public void Interact()
	{
		if (IsInteractable && _interactCallback != null)
		{
			_interactCallback.Invoke();
		}
	}
}
