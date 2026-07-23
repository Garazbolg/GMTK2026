using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    public Rigidbody2D rigidBody;
    
    public InputActionReference moveAction;
    public InputActionReference lookAction;
    public InputActionReference throwAction;
    public InputActionReference dodgeAction;

    public Weapon currentWeapon;
    
    public float speed;
    public float dodgeSpeed;
    public float dodgeDuration;
    public float dodgeCooldown;
    
    public Transform firePoint;
    
    public int currentAmmo;
    public float lastFireTime;
    public bool canMove = true;
    
    void Start()
    {
        if(currentWeapon != null)
        {
            currentWeapon.OnEquip(this);
        }
    }

    private void OnEnable()
    {
        EnableControls();
    }

    private void OnDisable()
    {
        DisableControls();
    }

    public void EnableControls()
    {
        moveAction.action.Enable();
        lookAction.action.Enable();
        throwAction.action.Enable();
        throwAction.action.performed += InputThrow;
        dodgeAction.action.Enable();
    }
    
    public void DisableControls()
    {
        moveAction.action.Disable();
        lookAction.action.Disable();
        throwAction.action.Disable();
        throwAction.action.performed -= InputThrow;
        dodgeAction.action.Disable();
    }

    void Update()
    {
        if (canMove)
        {
            Move(moveAction.action.ReadValue<Vector2>());
        }
        
        Look(lookAction.action.ReadValue<Vector2>());

        if(currentWeapon != null && (lastFireTime + (1f/currentWeapon.baseFrequency) < Time.time))
        {
            lastFireTime = Time.time;
            Fire();
        }
    }
    
    public void Move(Vector2 direction)
    {
        Vector3 move = direction.normalized;
        if(direction != Vector2.zero)
        {
            rigidBody.linearVelocity = move * speed;
        }
        else
        {
            rigidBody.linearVelocity = Vector3.zero;
        }
    }
    
    public void Look(Vector2 direction)
    {
        if(direction != Vector2.zero)
        {
            transform.right = direction.normalized;
        }
    }

    public void Fire()
    {
        if (currentAmmo > 0)
        {
            currentWeapon.Fire(currentAmmo, firePoint);
            currentAmmo--;
        }
        else
        {
            currentWeapon.FireEmpty(firePoint);
            currentWeapon.OnUnequip(this);
            currentWeapon = null;
            currentAmmo = 0;
        }
    }
    
    public void InputThrow(InputAction.CallbackContext context)
    {
        Throw();
    }
    
    public void Throw()
    {
        if (currentWeapon != null)
        {
            currentWeapon.Throw(firePoint);
            currentWeapon.OnUnequip(this);
            currentWeapon = null;
            currentAmmo = 0;
        }
    }
}