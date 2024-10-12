using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LifespanObject : MonoBehaviour
{
    [Header("Hover over an item to view its tooltip.")]
    [Tooltip("Duration of this object's lifespan in seconds.")]
    [SerializeField] private float duration = 5f;
    [Tooltip("Assign what happens to this object on expiry.")]
    [SerializeField] private ExpiryType expiryType = 0;
    [Tooltip("Any other UnityEvents assigned to call on object expiry.")]
    [SerializeField] private UnityEvent invokeOnExpire;

    private float activityTime = 0f;

    public enum ExpiryType
    {
        Destroy,
        Disable
    }

    private void OnEnable()
    {
        activityTime = 0f;
    }

    private void Update()
    {
        if (activityTime > duration)
        {
            invokeOnExpire.Invoke();
            switch (expiryType)
            {
                case ExpiryType.Destroy:
                    Destroy(gameObject);
                    break;
                case ExpiryType.Disable:
                    gameObject.SetActive(false);
                    break;
            }
        }

        activityTime += Time.deltaTime;
    }
}
