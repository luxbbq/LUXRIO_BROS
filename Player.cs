using Godot;
using System;

public partial class Player : CharacterBody2D
{
	
	[Export] public float Speed = 300.0f;
    [Export] public float jumpvelocity = -900.0f;
    [Export] public AnimatedSprite2D sprite2D;
    public bool isLeft = false;
    public bool isMoving = false;
    public bool isJumping = false;
    

    public AnimatedSprite2D livAnim;

    public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle(); /* isso é um call po sistema de physics do outro tipo de node,  o CHARACTERBODY2D nao tem sistema de physics instalado na root dele, 
    entao tem que pegear do ridgib ou algo assim pro physiscs funcionar */
  
    public override void _Ready()
    {
        livAnim = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    }
   
    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;
        // ADDS THE GRAVITY
        if (!IsOnFloor())
        {
            velocity.Y += gravity * (float)delta;
            
            if (!isJumping) 
            {
                isJumping = true;
                _jumping();
            }
        }
        else
        {
            if (isJumping) 
            {
                isJumping = false;
            }
        }

        // things for the jump
        if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
        {
            velocity.Y = jumpvelocity;     
            _jumping();
        }
  
        float direction = Input.GetAxis("moveLeft", "moveRight");
        if (direction != 0)
        {
            velocity.X = direction * Speed;
            if (!isJumping)
            {
                livAnim.Play("walking");
            }
        }
        else 
        {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, 12);
             if (!isJumping)
            {
                livAnim.Play("idle");
            }
        }

        Velocity = velocity;
        MoveAndSlide();
            
	}

    public override void _Process(double delta)
    {
       //facing direction
         if (Velocity.X != 0)
        {
            isLeft = Velocity.X < 0;
        }
        sprite2D.FlipH = isLeft;    
    }

    public void _jumping()
    {
        livAnim.Play("jumping");
    }
}
