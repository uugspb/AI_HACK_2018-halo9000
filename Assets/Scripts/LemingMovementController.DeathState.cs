public partial class LemingMovementController
{
    private class DeathState : ControllerState
    {
        public DeathState(LemingMovementController controller) : base(controller)
        {
        }

        public override void Start()
        {
            _controller._movementDirection = 0;
            _controller._verticalSpeed = 0;
            _controller.motion = 0;
        }
    }
}