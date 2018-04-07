using UnityEngine;

public partial class LemingMovementController
{
    private class JumpState : ControllerState
    {
        public JumpState(LemingMovementController controller) : base(controller)
        {
        }

        public override void Start()
        {
            _controller._verticalSpeed = _controller.JumpForce;
        }

        public override void Update()
        {			
            _controller._verticalSpeed += Physics2D.gravity.y * Time.fixedDeltaTime;
            _controllerRigidbody2D.MovePosition(
                _controllerRigidbody2D.position + new Vector2(_controller._movementDirection * _controller.AirControllSpeed,
                    _controller._verticalSpeed * Time.fixedDeltaTime));

            if (_controller.IsGrounded())
            {
                _controller.CurrentState = new IdleState(_controller);
            }
        }

        public override void Move(int direction)
        {
            if(direction != 0)
                _controller._movementDirection = direction;				
        }

        public override void End()
        {
            _controller._verticalSpeed = 0;
        }
    }
}