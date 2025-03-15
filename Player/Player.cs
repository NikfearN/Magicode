using Godot;
using System;



public partial class Player : CharacterBody2D
{
    [Export] public int Speed = 200;
    [Export] public InteractingComponent intcomp;

    public AnimationTree _animationTree;
    private AnimationNodeStateMachinePlayback _stateMachine;
    private Vector2 _lastdirection = Vector2.Down;
    private Vector2 inputVector = Vector2.Zero;
    private bool _isInteracting = false; // Флаг для блокировки движения

    public override void _Ready()
    {
        _animationTree = GetNode<AnimationTree>("AnimationTree");
        _animationTree.Active = true;
        _stateMachine = (AnimationNodeStateMachinePlayback)_animationTree.Get("parameters/playback");
        intcomp = GetNode<InteractingComponent>("InteractingComponent");
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (Input.IsActionJustPressed("interact") && intcomp._canInteract)
        {
            _isInteracting = true; 
            inputVector = Vector2.Zero; // Останавливаем игрока
            _isInteracting = false;
        }
        else if (!_isInteracting) // Если не взаимодействуем, разрешаем движение
        {
            inputVector = Input.GetVector("move_left", "move_right", "move_up", "move_down");
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_isInteracting)
        {
            Velocity = Velocity.MoveToward(Vector2.Zero, Speed);
        }
        else if (inputVector.LengthSquared() > 0)
        {
            Velocity = inputVector * Speed;
            _lastdirection = inputVector; // Обновляем последнее направление
        }
        else
        {
            Velocity = Velocity.MoveToward(Vector2.Zero, Speed);
        }

        MoveAndSlide();

        if (Velocity.LengthSquared() > 0)
        {
            _stateMachine.Travel("movement");
            _animationTree.Set("parameters/movement/BlendSpace2D/blend_position", Velocity);
        }
        else
        {
            _animationTree.Set("parameters/idle/BlendSpace2D/blend_position", _lastdirection);
            _stateMachine.Travel("idle");
        }
    }

}
