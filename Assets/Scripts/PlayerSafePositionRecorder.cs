using System.Collections;
using UnityEngine;

public class PlayerSafePositionRecorder : MonoBehaviour
{

    private PlayerMovement playerMovement;

    [SerializeField] private float waitAfterDangerSeconds = 0.5f;

    private bool dangerLock = false;

    private Vector3 safePosition;
    public Vector3 SafePosition => safePosition;
    [SerializeField] private Transform[] safetyCheckTransforms;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    private bool IsSafe()
    {
        bool safe = true;

        foreach (var tform in safetyCheckTransforms)
        {
            Collider2D col = Physics2D.OverlapCircle(tform.position, 0.1f, layerMask: LayerMask.GetMask("Ground"));
            if (!col || col.CompareTag("Danger"))
            {
                safe = false;
            }
        }
        return safe;
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
