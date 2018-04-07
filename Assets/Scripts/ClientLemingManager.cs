using UnityEngine;

public class ClientLemingManager : MonoBehaviour
{

    public LemingMovementController Leming;
    [SerializeField] private int _input;

    private void Update()
    {
        _input = 0;
        var horizontal = Input.GetAxis("Horizontal");
        _input |= (int)((horizontal > 0) ? EInput.MoveRigth : horizontal < 0 ? EInput.MoveLeft : EInput.None);
        var vertical = Input.GetAxis("Vertical");
        _input |= (int) (vertical > 0 ? EInput.Jump : EInput.None);
    }

    private void FixedUpdate()
    {
        Leming.ManualFixedUpdate(_input);
    }
}