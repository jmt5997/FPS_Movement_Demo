using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public GunData gunData;
    public Camera playerCamera;
    private float timeSinceLastShot = 0;
    public GameObject audioPrefab, muzzleFlash, flashLight;
    public Transform muzzle;
    public Animator anim;

    // Start is called before the first frame update
    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastShot += Time.deltaTime;
        if(timeSinceLastShot > gunData.reloadTime && gunData.reloading)
        {
            gunData.reloading = false;
        }
    }

    private bool canShoot() => !gunData.reloading && timeSinceLastShot > 1f / (gunData.fireRate / 60f);

    public void shoot()
    {
        if(gunData.currentAmmo > 0 && canShoot())
        {
            if(Physics.Raycast(playerCamera.transform.position, transform.forward, out RaycastHit hitInfo, gunData.maxDistance))
            {
                Debug.Log(hitInfo.transform.name);
            }
            gunData.currentAmmo--;
            timeSinceLastShot = 0;
            onGunShot();
        }

    }

    void onGunShot()
    {
        Destroy(Instantiate(audioPrefab), 3.0f);
        Destroy(Instantiate(muzzleFlash, muzzle.position, muzzle.rotation), 0.1f);
        Destroy(Instantiate(flashLight, playerCamera.transform.position, playerCamera.transform.rotation), 0.1f);
        anim.Play("Fire", -1, 0f);
    }

    public void reload()
    {
        if(!gunData.reloading)
        {
            anim.Play("Reload");
            gunData.reloading = true;
            timeSinceLastShot = 0;
            gunData.currentAmmo = gunData.magSize;
        }
    }
}
