using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Makes the object with this attached script a billboard, which means this object's rotation will match the camera's.
/// </summary>
public class BillboardObject : MonoBehaviour
{
    [SerializeField] private bool matchRotationOnStart = true;
    [SerializeField] private bool matchRotationOnUpdate;
    [SerializeField] private bool matchRotationOnLateUpdate;
    [SerializeField] private bool matchRotationOnFixedUpdate;
    [SerializeField] private bool lockX, lockY, lockZ;
    private Vector3 targetRotation;

    private void Start()
    {
        if (matchRotationOnStart) MatchRotation();
    }

    private void Update()
    {
		if (matchRotationOnUpdate) MatchRotation();
    }

    private void LateUpdate()
    {
        if (matchRotationOnLateUpdate) MatchRotation();
    }

    private void FixedUpdate()
    {
        if (matchRotationOnFixedUpdate) MatchRotation();
    }

    public void MatchRotation()
	{
        targetRotation = Camera.main.transform.rotation.eulerAngles;
        if (lockX) targetRotation.x = transform.rotation.eulerAngles.x;
        if (lockY) targetRotation.y = transform.rotation.eulerAngles.y;
        if (lockZ) targetRotation.z = transform.rotation.eulerAngles.z;
        transform.rotation = Quaternion.Euler(targetRotation);
	}
}
