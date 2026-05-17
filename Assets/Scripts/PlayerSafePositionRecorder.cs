using System.Collections;
using UnityEngine;

public class PlayerSafePositionRecorder : MonoBehaviour
{
    [SerializeField] private float waitAfterDangerSeconds = 0.5f;

    private bool dangerLock = false;

    private Vector3 safePosition;
    public Vector3 SafePosition => safePosition;
    [SerializeField] private Transform[] safetyCheckTransforms;

    private int groundLayerMask;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        groundLayerMask = LayerMask.GetMask("Ground");
    }

    private bool IsSafe()
    {
        foreach (var tform in safetyCheckTransforms)
        {
            Collider2D col = Physics2D.OverlapCircle(tform.position, 0.1f, groundLayerMask);
            if (!col || col.CompareTag("Danger"))
            {
                return false;
            }
        }
        return true;
    }


    // Update is called once per frame
    void Update()
    {
        if (dangerLock) return;
        if (IsSafe())
        {
            safePosition = transform.position;
        }
        else
        {
            StartCoroutine(DangerLockCoroutine());
        }
    }


    private IEnumerator DangerLockCoroutine()
    {
        dangerLock = true;
        yield return new WaitForSeconds(waitAfterDangerSeconds);
        dangerLock = false;
    }

}
