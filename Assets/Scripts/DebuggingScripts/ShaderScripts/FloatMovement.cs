using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatMovement1 : MonoBehaviour
{
    [SerializeField] private float sensitivity;
    [SerializeField] private float speed;
    private float _mouseX;
    private float _mouseY;
    private float _inputX;
    private float _inputY;
    private float rotationX;
    private float rotationY;
    private float _jumpInput;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        _mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        _mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
        _inputX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        _inputY = Input.GetAxis("Vertical")* speed * Time.deltaTime;
        _jumpInput = Input.GetAxis("Jump") * speed * Time.deltaTime;

        transform.Translate(_inputX, _jumpInput, _inputY);
        rotationX += _mouseX;
        rotationY += _mouseY;
        transform.rotation = Quaternion.Euler(rotationY, -rotationX, 0);
    }
}
