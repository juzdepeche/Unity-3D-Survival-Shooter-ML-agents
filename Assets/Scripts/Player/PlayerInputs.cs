using UnityEngine;

public class PlayerInputs : MonoBehaviour
{
    public float HorizontalAxis => _horizontalAxis;
    private float _horizontalAxis = 0f;

    public float VerticalAxis => _verticalAxis;
    private float _verticalAxis = 0f;

    public bool IsShooting => _isShooting;
    private bool _isShooting = false;

    private void Update()
    {
        _horizontalAxis = Input.GetAxisRaw("Horizontal");
        _verticalAxis = Input.GetAxisRaw("Vertical");

        _isShooting = Input.GetButton("Fire1");
    }
}
