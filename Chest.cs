using Godot;

public partial class Chest : StaticBody2D
{
	[Export]
	private Area2D Interactable;



	[Export]
	private Sprite2D sprite;

	public override void _Ready()
	{
		Interactable = GetNode<Interactable>("Interactable");
		sprite = GetNode<Sprite2D>("Sprite2D");
		// Подписываемся на событие взаимодействия
		if (Interactable is Interactable interactableComponent)
		{
			interactableComponent.SetInteract(OnInteract);
		}
	}

	private void OnInteract()
	{
		GD.Print("OnInteract called");
		if (sprite.Frame == 0)
		{
			sprite.Frame = 1; // Изменяем кадр спрайта
			if (Interactable is Interactable interactableComponent)
			{
				interactableComponent.IsInteractable = false; // Делаем сундук неактивным для взаимодействия
			}
			GD.Print("The player gained 10 gold"); // Выводим сообщение в консоль
		}
	}
}
