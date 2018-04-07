public partial class LemingMovementController
{
    private class IdleState : ControllerState
    {
        public IdleState(LemingMovementController controller) : base(controller)
        {
        }

        public override void Move(int direction)
        {
            if (direction != 0)
            {
                _controller.CurrentState = new MoveState(_controller);
				
            }
            _controller._movementDirection = direction;
        }
		
        public override void Jump()
        {
            _controller._verticalSpeed = _controller.JumpForce;
            _controller.CurrentState = new JumpState(_controller);
            _controller.OnJump();
        }

        public override void Update()
        {
            if (!_controller.isGrounded)
            {
                _controller.CurrentState = new JumpState(_controller);
            }
        }
    }
}