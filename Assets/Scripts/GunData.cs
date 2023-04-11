using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GunData", menuName = "Weapon/Gun", order = 0)]
public class GunData : ScriptableObject
{
    [Header("Info")]
    public float damage;
    public float maxDistance;
    public int currentAmmo;
    public int magSize;
    public float fireRate;
    public float reloadTime;
    public bool reloading;
    public AudioClip fireSound;
}