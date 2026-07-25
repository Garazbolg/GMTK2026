using System;
using System.Collections;
using DevCore.ScriptableVariables;
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
    
    public ScriptableFloat totalDurationVariable;
    public ScriptableFloat currentDurationVariable;
    public ScriptableFloat currentFrequencyVariable;

    public Weapon currentWeapon;
    
    public float speed;
    public float dodgeSpeed;
    public float dodgeDuration;
    public AnimationCurve throwKickBackCurve;
    
    public Transform firePoint;

    public float currentDuration;
    public float lastFireTime;
    public bool canMove = true;
    public int currentSequenceIndex;
    
    public bool DEBUG_NEVERREDUCEDURATION;

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
            if(!DEBUG_NEVERREDUCEDURATION)
                currentDuration -= Time.deltaTime;
            if(durationText != null)
                durationText.text = currentDuration.ToString("0.0");
            if(currentDuration <= 0)
            {
                currentWeapon.FireEmpty(transform);
                currentWeapon.OnUnequip(this);
                currentWeapon = null;
            }
        }
        else
        {
            if(durationText != null)
                durationText.text = "";
        }
        
        currentDurationVariable.value = currentWeapon != null ? currentDuration : 3;
        currentFrequencyVariable.value = currentWeapon != null ? currentWeapon.baseFrequency : 3;
        totalDurationVariable.value = currentWeapon != null ? currentWeapon.duration : 3;
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
        currentWeapon.Fire(currentSequenceIndex, firePoint);
        EnemyFireTick();
        currentSequenceIndex++;
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
