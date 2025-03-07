using Godot;
using System;

public partial class Interactable : Area2D
{
	[Export]
	public string InteractName { get; set; } = "";

	[Export]
	public bool IsInteractable { get; set; } = true;

	private Action _interactCallback;

	// Метод для установки callback-функции
	public void SetInteract(Action interactCallback)
	{
		_interactCallback = interactCallback;
	}

	// Метод, который вызывается при взаимодействии
	public void Interact()
	{
		if (IsInteractable && _interactCallback != null)
		{
			_interactCallback.Invoke();
		}
	}
}
