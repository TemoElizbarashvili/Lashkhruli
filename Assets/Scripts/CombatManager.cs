using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance;

    public bool CanReceiveInput = true;
    public bool InputReceived;

    private void Awake()
    {
        CanReceiveInput = true;
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        Attack();
    }

    private void Attack()
    {
        if (!Input.GetMouseButtonDown(0))
        {
            return;
        }

        if (!CanReceiveInput)
            return;

        InputReceived = true;
        CanReceiveInput = false;
    }

    public void InputManager()
        => CanReceiveInput = !CanReceiveInput;
}
