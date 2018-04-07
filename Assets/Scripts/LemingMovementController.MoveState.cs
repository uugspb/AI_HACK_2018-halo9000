using UnityEngine;

public partial class LemingMovementController
{
    private class MoveState : ControllerState
    {
        public MoveState(LemingMovementController controller) : base(controller)
        {
        }

        public override void Move(int direction)
        {
            if (direction == 0)
            {
                _controller.CurrentState = new IdleState(_controller);
            }
            _controller._movementDirection = direction;				
        }

        public override void Jump()
        {
            _controller._verticalSpeed = _controller.JumpForce;
            _controller.CurrentState = new JumpState(_controller);
        }

        public override void Update()
        {
            _controllerRigidbody2D.MovePosition(_controllerRigidbody2D.position +
                                                new Vector2(_controller._movementDirection * _controller.Speed,
                                                    0)); 
            if (!_controller.isGrounded)
            {
                _controller.CurrentState = new JumpState(_controller);
            }
        }
		
		
    }
}