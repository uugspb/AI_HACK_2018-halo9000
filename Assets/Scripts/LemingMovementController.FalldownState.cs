public partial class LemingMovementController
{
    private class FalldownState : JumpState
    {
        public FalldownState(LemingMovementController controller) : base(controller)
        {
        }

        public override void Start()
        {
        }
    }
}