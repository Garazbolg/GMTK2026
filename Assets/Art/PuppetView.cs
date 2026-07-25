using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PuppetView : MonoBehaviour
{
    [Header("Binding")]
    [SerializeField] private SpriteRenderer weaponSpriteRight;
    [SerializeField] private SpriteRenderer weaponSpriteLeft;
    [SerializeField] private Transform weaponPivot;
    [SerializeField] private Transform weaponFeedbackPivot;
    [SerializeField] private Transform projectileSpawn;
    [SerializeField] private Transform debug;
    [Header("Feel")]
    [SerializeField] public float recoilStrength = 0.1f;
    [SerializeField] public float recoilDuration = 0.2f;

    #region Gameplay API
    public Vector3 GetProjectileSpawnPosition() => projectileSpawn.position;

    public void AimPosition(Vector3 worldPosition)
    {
        Vector3 pivotPosition = NoZ(weaponPivot.position);
        var targetDirection = (worldPosition - pivotPosition).normalized;
        AimDirection(targetDirection);
    }

    public void AimDirection(Vector3 worldDirection)
	{
		var angle = Vector3.SignedAngle(new Vector3(1,0,0), worldDirection, Vector3.forward);

        bool lookRight = Mathf.Abs(angle) <= 90.0f;
        weaponSpriteRight.enabled = lookRight;
        weaponSpriteLeft.enabled = !lookRight;

        weaponPivot.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}

    public void PlayShotFeedback(float recoilStrength)
    {
        weaponFeedbackPivot.DOKill();
        weaponFeedbackPivot.transform.localPosition = Vector3.zero; // need reset (dotween does not cumulate punches?)
        weaponFeedbackPivot.DOPunchPosition(-Vector3.right * recoilStrength, recoilDuration);
    }

    public void SetExplosionNearFeedback(float ratio)
    {
        ratio = Mathf.Clamp(ratio, 0f, 1f);
        if (ratio <= 0)
        {
            weaponSpriteRight.color = Color.white;
            weaponSpriteLeft.color = Color.white;
        } else
        {
            var color = Color.Lerp(Color.white, Color.red, ratio);
            weaponSpriteRight.color = color;
            weaponSpriteLeft.color = color;
        }
    }
    #endregion

    void Start()
    {
        weaponSpriteRight.gameObject.SetActive(true);
        weaponSpriteLeft.gameObject.SetActive(true);
        weaponSpriteRight.enabled = true;
        weaponSpriteLeft.enabled = false;
    }

    void Update()
	{
		TestMouseAimDirection();
        if(Mouse.current.leftButton.wasPressedThisFrame)
        {
            PlayShotFeedback(recoilStrength);
        }
    }

    void TestMouseAimDirection()
    {
        Vector3 targetPosition = GetMouseWorldPos();
        AimPosition(targetPosition);
        debug.position = targetPosition;
    }

    static Vector3 GetMouseWorldPos()
	{
        Vector3 targetPosition = Mouse.current.position.ReadValue();
        targetPosition.z = Camera.main.nearClipPlane;
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(targetPosition);
        mouseWorldPos.z = 0;
        return mouseWorldPos;
    }

    static Vector3 NoZ(Vector3 pos)
    {
        pos.z = 0;
        return pos;
    }
}
