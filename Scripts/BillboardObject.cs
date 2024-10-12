using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Makes the object with this attached script a billboard, which means this object's rotation will match the camera's.
/// </summary>
public class BillboardObject : MonoBehaviour
{
    public bool lockX, lockY, lockZ;
    private Vector3 targetRotation;

    private void Update()
    {
		MatchRotation();
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
