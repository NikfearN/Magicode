using Godot;
using System;
using System.Collections.Generic;

public partial class InteractingComponent : Node2D
{
	[Export]
	private Label InteractLabel;

	private List<Interactable> _currentInteractions = new List<Interactable>();
	private bool _canInteract = true;

	public override void _Ready()
	{
		// Получаем узел InteractRange
		Area2D interactRange = GetNode<Area2D>("InteractRange");
		InteractLabel = GetNode<Label>("InteractLabel");
		// Подключаем сигналы area_entered и area_exited
		interactRange.Connect("area_entered", new Callable(this, nameof(OnInteractRangeAreaEntered)));
		interactRange.Connect("area_exited", new Callable(this, nameof(OnInteractRangeAreaExited)));
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("interact") && _canInteract)
		{
			GD.Print("Interaction called");
			if (_currentInteractions.Count > 0)
			{
				_canInteract = false;
				InteractLabel.Hide();
				// Вызываем взаимодействие
				CallInteract();
			}
		}
	}

	private void CallInteract()
	{
		if (_currentInteractions.Count > 0)
		{
			_currentInteractions[0].Interact(); // Вызываем метод Interact
			_canInteract = true;
		}
	}

public override void _PhysicsProcess(double delta)
{
	// Проверка, что _currentInteractions не равен null
	if (_currentInteractions == null)
	{
		return;
	}

	// Проверка, что InteractLabel не равен null
	if (InteractLabel == null)
	{
		return;
	}

	// Проверка, что _canInteract равно true
	if (_currentInteractions.Count > 0 && _canInteract)
	{
		// Сортируем объекты по расстоянию
		_currentInteractions.Sort(SortByNearest);

		// Проверяем, что объект не null и доступен для взаимодействия
		if (_currentInteractions[0] != null && _currentInteractions[0].IsInteractable)
		{
			// Устанавливаем текст и показываем Label
			InteractLabel.Text = _currentInteractions[0].InteractName;
			InteractLabel.Show();
		}
	}
	else
	{
		// Скрываем Label, если нет объектов для взаимодействия
		InteractLabel.Hide();
	}
}


	private int SortByNearest(Interactable area1, Interactable area2)
	{
		if (area1 == null || area2 == null)
			return 0;

		float area1Dist = GlobalPosition.DistanceTo(area1.GlobalPosition);
		float area2Dist = GlobalPosition.DistanceTo(area2.GlobalPosition);
		return area1Dist.CompareTo(area2Dist);
	}

	private void OnInteractRangeAreaEntered(Area2D area)
{
	GD.Print("Area entered: " + area.Name);
	if (area is Interactable interactable)
	{
		_currentInteractions.Add(interactable);
	}
}

private void OnInteractRangeAreaExited(Area2D area)
{
	GD.Print("Area exited: " + area.Name);
	if (area is Interactable interactable)
	{
		_currentInteractions.Remove(interactable);
	}
}
}
