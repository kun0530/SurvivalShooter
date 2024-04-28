using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class PlayerInput : MonoBehaviour
{
    public enum Controller
    {
        KeyboadMouse,
        GamePad,
        Count
    }
    private Controller controller = Controller.KeyboadMouse;

    public static readonly string moveAxisZName = "Vertical";
    public static readonly string moveAxisXName = "Horizontal";
    public static readonly string rotateAxisZName = "VerticalRotate";
    public static readonly string rotateAxisXName = "HorizontalRotate";

    public static readonly string fireButtonName = "Fire1";

    private Plane plane = new Plane(Vector3.up, Vector3.zero);

    public Vector3 move { get; private set; }
    public Vector3 rotate { get; private set; }
    public bool fire { get; private set; }

    private void Update()
    {
        // 공통
        move = new Vector3(Input.GetAxis(moveAxisXName), 0f, Input.GetAxis(moveAxisZName));

        switch (controller)
        {
            case Controller.KeyboadMouse:
                {
                    var cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                    float rayLength;
                    if (plane.Raycast(cameraRay, out rayLength))
                    {
                        rotate = cameraRay.GetPoint(rayLength);
                    }
                    break;
                }
            case Controller.GamePad:
                {
                    rotate = transform.position + new Vector3(Input.GetAxis(rotateAxisXName), 0f, Input.GetAxis(rotateAxisZName));
                    break;
                }
        } 

        fire = Input.GetButton(fireButtonName);
    }

    public void ChangeController(int controller)
    {
        this.controller = (Controller)controller;
    }
}
