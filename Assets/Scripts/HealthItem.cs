using UnityEngine;

public class HealthItem : MonoBehaviour
{
    public Animator Animator;

    void Start()
    {
        Animator = GetComponent<Animator>();
    }

    public void Destroy()
        => Animator.SetTrigger("Destroy");

    public void Destroyed()
    {
        Destroy(gameObject);
    }
}
