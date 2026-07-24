using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    public Rigidbody2D rigidBody;
    public TMPro.TextMeshProUGUI durationText;
    
    public InputActionReference moveAction;
    public InputActionReference lookAction;
    public InputActionReference throwAction;
    public InputActionReference dodgeAction;

    public Weapon currentWeapon;
    
    public float speed;
    public float dodgeSpeed;
    public float dodgeDuration;
    public AnimationCurve throwKickBackCurve;
    
    public Transform firePoint;

    public float currentDuration;
    public int currentAmmo;
    public float lastFireTime;
    public bool canMove = true;

    public float minThrowDuration;
    
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
            var moveDirection = moveAction.action.ReadValue<Vector2>();
            Move(moveDirection);
            var lookDirection = lookAction.action.ReadValue<Vector2>();
            Look(currentWeapon != null ? lookDirection : moveDirection);
        }

        if(currentWeapon != null && (lastFireTime + (1f/currentWeapon.baseFrequency) < Time.time))
        {
            lastFireTime = Time.time;
            Fire();
        }
        
        if(currentWeapon != null && currentDuration > 0)
        {
            currentDuration -= Time.deltaTime;
            durationText.text = currentDuration.ToString("0.0");
            if(currentDuration <= 0)
            {
                currentWeapon.FireEmpty(transform);
                currentWeapon.OnUnequip(this);
                currentWeapon = null;
                currentAmmo = 0;
            }
        }
        else
        {
            durationText.text = "";
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
            EnemyFireTick();
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
            currentWeapon.Throw(firePoint, Mathf.Max(currentDuration, minThrowDuration));
            currentWeapon.OnUnequip(this);
            currentWeapon = null;
            currentAmmo = 0;
            KnockBack(-transform.right * dodgeSpeed, dodgeDuration);
        }
    }

    public void EnemyFireTick()
    {
        var enemies = FindObjectsByType<EnemyFireTickResponseBehaviour>(FindObjectsSortMode.None);
        foreach (var enemyFireTickResponseBehaviour in enemies)
        {
            enemyFireTickResponseBehaviour.OnFireTick();
        }
    }

    public void KnockBack(Vector2 knockBackDirection, float knockBackDuration)
    {
        StartCoroutine(KnockBackCoroutine(knockBackDirection, knockBackDuration));
    }

    private IEnumerator KnockBackCoroutine(Vector2 knockBackDirection, float knockBackDuration)
    {
        float elapsedTime = 0f;
        canMove = false;
        while (elapsedTime < knockBackDuration)
        {
            rigidBody.linearVelocity = knockBackDirection * throwKickBackCurve.Evaluate(elapsedTime / knockBackDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        canMove = true;
        rigidBody.linearVelocity = Vector3.zero;
    }
}
