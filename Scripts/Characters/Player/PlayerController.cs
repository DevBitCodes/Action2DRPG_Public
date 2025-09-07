using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterMovement characterMovement;
    private CharacterCombat characterCombat;
    private InputSystem inputSystem;

    private void OnEnable()
    {
        inputSystem?.Enable();
    }

    private void OnDisable()
    {
        inputSystem?.Disable();
    }

    private void Awake()
    {
        characterMovement = GetComponent<CharacterMovement>();
        characterCombat = GetComponent<CharacterCombat>();
        inputSystem = new InputSystem();
    }

    private void Start()
    {
        inputSystem.Combat.Attack.started += _ => Attack();
    }

    private void Update()
    {
        characterMovement.SetDirection(inputSystem.Movement.Direction.ReadValue<Vector2>());
    }

    private void Attack()
    {
        characterCombat.Attack();
    }
}
