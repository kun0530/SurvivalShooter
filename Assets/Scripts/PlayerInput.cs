using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerInput : MonoBehaviour
{
    public static readonly string moveAxisZName = "Vertical";
    public static readonly string moveAxisXName = "Horizontal";
    public static readonly string fireButtonName = "Fire1";

    private Plane plane = new Plane(Vector3.up, Vector3.zero);

    public Vector3 move { get; private set; }
    public Vector3 rotate { get; private set; }
    public bool fire { get; private set; }

    private void Update()
    {
        move = new Vector3(Input.GetAxisRaw(moveAxisXName), 0f, Input.GetAxisRaw(moveAxisZName)).normalized;

        var cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        float rayLength;
        if (plane.Raycast(cameraRay, out rayLength))
        {
            rotate = cameraRay.GetPoint(rayLength);
        }
        // var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // Physics.Raycast(ray. out RaycastHit hitInfo, LayerMask.GetMask("Floor"));
        // transform.LookAt(new Vector3(hitInfo.point.x, transform.positoin.y, hitInfo.point.z));

        fire = Input.GetButton(fireButtonName);
    }
}
