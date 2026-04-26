using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerSafePositionRecorder : MonoBehaviour
{

    private PlayerMovement playerMovement;

    [SerializeField] private float waitAfterDangerSeconds = 0.5f;

    private bool dangerLock = false;

    private Vector3 safePosition;
    public Vector3 SafePosition => safePosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }



    // Update is called once per frame
    void Update()
    {
        if (dangerLock) return;

        if (playerMovement.playerState == PlayerState.Idle
            || playerMovement.playerState == PlayerState.Running)
        {
            safePosition = playerMovement.transform.position;
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
