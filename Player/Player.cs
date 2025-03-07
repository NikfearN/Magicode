using Godot;
using System;

public partial class Player : CharacterBody2D
{
[Export] public int Speed = 200;

public AnimationTree _animationTree;
private AnimationNodeStateMachinePlayback _stateMachine;
private Vector2 _lastdirection = Vector2.Down;

public override void _Ready()
	{
		_animationTree = GetNode<AnimationTree>("AnimationTree");
		_animationTree.Active = true;
		_stateMachine = (AnimationNodeStateMachinePlayback)_animationTree.Get("parameters/playback");
	}

public override void _PhysicsProcess(double delta)
{
Vector2 velocity = new Vector2();

if (Input.IsActionPressed("move_right"))
{
velocity.X += 1;
}
if (Input.IsActionPressed("move_left"))
{
velocity.X -= 1;
}
if (Input.IsActionPressed("move_down"))
{
velocity.Y += 1;
}
if (Input.IsActionPressed("move_up"))
{
velocity.Y -= 1;
}

if (velocity.Length() > 0)
{
_stateMachine.Travel("movement");
_animationTree.Set("parameters/movement/BlendSpace2D/blend_position", velocity);
velocity = velocity.Normalized() * Speed;
_lastdirection = velocity;
}
else{
_stateMachine.Travel("idle");
_animationTree.Set("parameters/idle/BlendSpace2D/blend_position", _lastdirection);
}
Velocity = velocity;
MoveAndSlide();
}
}
