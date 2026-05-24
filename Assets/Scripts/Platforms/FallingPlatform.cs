using System.Collections;
using UnityEngine;
using UnityEngine.Assertions.Must;

[RequireComponent(typeof(Animator))]
public class FallingPlatform : MonoBehaviour
{
    [SerializeField] private float recoveryTime = 8.0f;
    private static readonly int FallHash = Animator.StringToHash("Fall");
    private static readonly int RecoverHash = Animator.StringToHash("Recover");
    private Animator animator;
    private bool interacted = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private IEnumerator RecoveryCoroutine()
    {
        yield return new WaitForSeconds(recoveryTime);
        animator.Play(RecoverHash);
        interacted = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (interacted || !other.CompareTag("Player")) return;
        interacted = true;
        animator.Play(FallHash);
        StartCoroutine(RecoveryCoroutine());
    }
}
