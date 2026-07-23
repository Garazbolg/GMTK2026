using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class Weapon : ScriptableObject
{
    public float baseFrequency;
    public float timeBeforeFirstFire;

    public GameObject weaponVisual;
    
    public Bullet[] bulletPrefabs;
    public GameObject onThrow;
    public GameObject onEmptyInHand;
    public GameObject onDrop;
    
    public Gradient ammoGradient;
    
    public int ammo => bulletPrefabs.Length;

    public void OnEquip(CharacterController controller)
    {
        if(controller.currentWeapon != null)
        {
            controller.currentWeapon.OnUnequip(controller);
        }
        controller.currentWeapon = this;
        controller.currentAmmo = ammo;
        controller.lastFireTime = Time.time + (timeBeforeFirstFire - 1f/baseFrequency);
        Instantiate(weaponVisual, controller.firePoint.position, controller.firePoint.rotation, controller.firePoint);
    }

    public void OnUnequip(CharacterController controller)
    {
        if (controller.firePoint.childCount > 0)
        {
            Destroy(controller.firePoint.GetChild(0).gameObject);
        }
    }

    public Color GetAmmoColor(int ammoIndex)
    {
        return ammoGradient.Evaluate(ammoIndex / ((float)ammo-1));
    }
    
    public void Fire(int currentAmmo, Transform firePoint)
    {
        Instantiate(bulletPrefabs[ammo-currentAmmo], firePoint.position, firePoint.rotation);
    }
    
    public void FireEmpty(Transform firePoint)
    {
        if (onEmptyInHand != null)
        {
            Instantiate(onEmptyInHand, firePoint.position, firePoint.rotation);
        }
    }
    
    public void Throw(Transform throwPoint)
    {
        if (onThrow != null)
        {
            Instantiate(onThrow, throwPoint.position, throwPoint.rotation);
        }
    }
}