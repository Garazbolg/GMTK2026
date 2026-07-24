using UnityEngine;

public class WeaponLoot : MonoBehaviour
{
    public Weapon weapon;

    public void OnLoot(GameObject character)
    {
        if (character.CompareTag("Player"))
        {
            var controller = character.GetComponent<CharacterController>();
            if (controller.currentWeapon != null) return;
            weapon.OnEquip(controller);
            Destroy(gameObject);
        }
    }
}