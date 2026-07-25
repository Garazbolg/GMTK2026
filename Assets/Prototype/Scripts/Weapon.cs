using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class Weapon : ScriptableObject
{
    public float duration;
    public float baseFrequency;
    public float timeBeforeFirstFire;

    //public GameObject weaponVisual;
    
    public GameObject onThrow;
    public GameObject onEmptyInHand;
    public GameObject onDrop;
    
    public Gradient ammoGradient;
    
    [Serializable]
    public struct FireOrigin
    {
        public Bullet bulletPrefab;
        public Vector2 position;
        public float rotation;
    }
    
    [Serializable]
    public struct FireSalve
    {
        public FireOrigin[] fireOrigins;
    }
    
    public FireSalve[] fireSequence;

    public void OnEquip(CharacterController controller)
    {
        if(controller.currentWeapon != null)
        {
            controller.currentWeapon.OnUnequip(controller);
        }
        controller.currentSequenceIndex = 0;
        controller.currentDuration = duration;
        controller.currentWeapon = this;
        controller.lastFireTime = Time.time + (timeBeforeFirstFire - 1f/baseFrequency);
        //Instantiate(weaponVisual, controller.firePoint.position, controller.firePoint.rotation, controller.firePoint);
        // TODO Set Weapon Sprite
    }

    public void OnUnequip(CharacterController controller)
    {
        if (controller.firePoint.childCount > 0)
        {
            Destroy(controller.firePoint.GetChild(0).gameObject);
        }
    }
    
    public void Fire(int fireSequenceIndex, Transform firePoint)
    {
        var fireSalve = fireSequence[fireSequenceIndex%fireSequence.Length];
        foreach (var fireOrigin in fireSalve.fireOrigins)
        {
            Instantiate(fireOrigin.bulletPrefab, firePoint.position + firePoint.TransformDirection(fireOrigin.position), firePoint.rotation * Quaternion.Euler(0, 0, fireOrigin.rotation));
        }
    }
    
    public void FireEmpty(Transform firePoint)
    {
        if (onEmptyInHand != null)
        {
            Instantiate(onEmptyInHand, firePoint.position, firePoint.rotation);
        }
    }
    
    public void Throw(Transform throwPoint, float remainingDuration)
    {
        if (onThrow != null)
        {
            var go = Instantiate(onThrow, throwPoint.position, throwPoint.rotation);
            go.GetComponent<AfterSecondsBecomes>().secondsBecomes = remainingDuration;
        }
    }
}