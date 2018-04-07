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
            _controller.CurrentState = new JumpState(_controller);
        }

        public override void Update()
        {
            if (!_controller.IsGrounded())
            {
                _controller.CurrentState = new FalldownState(_controller);
            }
        }
    }
}