using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;

public class Player : MonoBehaviour
{
    public PhotonView photonView;
    public Rigidbody2D rigidbody;
    public Animator animator;
    public GameObject playerCamera;
    public SpriteRenderer renderer;
    public Text playerNameText;
    public float moveSpeed;
    [SerializeField] private AudioSource walkingSource;
    [SerializeField] private AudioSource reloadSource;
    [SerializeField] private AudioSource shootingSource;
    bool walkSoundPlaying;

    // variables for the shoot code
    public GameObject bulletPrefab;
    private string bulletPrefabName;
    public Transform bulletStart;
    public Transform gun;
    public Transform gunPivotPoint;
    GameObject bullet;
    private int currentAmmo;
    [SerializeField] private int maxAmmo;
    private Text ammoText;
    Vector2 movement;

    Vector3 target;
    Vector3 difference;
    float distance;
    float rotationZ;
    Quaternion gunRotation;
    Vector2 direction;
    public Sprite[] weaponSprites;
    bool automaticWeapon;
    float timeUntilCanShoot = 0;
    float shootDelay;

    bool burstWeapon;
    int burstAmount;
    int burstAmountLeft;
    bool burstCurrentlyShooting = false;



    private void Awake(){
        if(photonView.IsMine){
            playerCamera.SetActive(true);
            //InitialiseWeapon(); // initialises variables for the players weapon
            playerNameText.text = PhotonNetwork.NickName;
            currentAmmo = maxAmmo;
        }else{
            playerNameText.text = photonView.Owner.NickName;
            playerNameText.color = Color.blue;
        }
    }

    private void Start(){
        ammoText = GameObject.Find("AmmoText").GetComponent<Text>();
        SetAmmoText();
        walkSoundPlaying = false;
        bulletPrefabName = bulletPrefab.name;
        automaticWeapon = false;
    }

    private void Update(){
        if(photonView.IsMine){
            CheckInput();

            target = GameObject.FindObjectOfType<Camera>().ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z));
            difference = target - gunPivotPoint.transform.position;
            distance = difference.magnitude;
            rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            gunRotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
            photonView.RPC("UpdateGunAim", RpcTarget.All, gunRotation);
            direction = difference / distance;
        }
    }
    private void FixedUpdate(){
        if(photonView.IsMine){
            movement.x = Input.GetAxis("Horizontal");
            movement.y = Input.GetAxis("Vertical");
            WalkingSound(movement.x, movement.y);
            rigidbody.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, Input.GetAxisRaw("Vertical") * moveSpeed);
            gameObject.GetComponent<Animator>().SetFloat("Horizontal", movement.x);
            gameObject.GetComponent<Animator>().SetFloat("Vertical", movement.y);
            gameObject.GetComponent<Animator>().SetFloat("Speed", movement.sqrMagnitude);
        }
    }

    [PunRPC]
    private void UpdateGunAim(Quaternion gunRot){
        gunPivotPoint.transform.rotation = gunRot;
    }



    private void CheckInput(){
        /*
        if(Input.GetKeyDown(KeyCode.A)){
            photonView.RPC("FlipTrue", RpcTarget.AllBuffered);
        }
        if(Input.GetKeyDown(KeyCode.D)){
            photonView.RPC("FlipFalse", RpcTarget.AllBuffered);
        }*/
        if(automaticWeapon){
            if(Input.GetKey(KeyCode.Mouse0)){
                PullTrigger();
            }
        }else{
            if(Input.GetKeyDown(KeyCode.Mouse0)){
                if(burstWeapon){
                    if(!burstCurrentlyShooting){
                        burstCurrentlyShooting = true;
                        burstAmountLeft = burstAmount;
                    }
                }else{
                    PullTrigger();
                }
            }
        }
        if(burstCurrentlyShooting){
            BurstPullTrigger();
            if(burstAmountLeft <= 0){
                burstCurrentlyShooting = false;
            }
        }
        timeUntilCanShoot -= Time.deltaTime;

        if(Input.GetKeyDown(KeyCode.Backspace)){
            gameObject.GetComponent<HealthSystem>().TakeDamage(1);
        }
        if(Input.GetKeyDown(KeyCode.R)){
            StartCoroutine(ReloadAmmo());
        }
    }

    private void PullTrigger(){
        if(currentAmmo > 0 && timeUntilCanShoot <= 0){
            Debug.Log("Shot fired");
            Shoot();
            timeUntilCanShoot = shootDelay;
        }
    }
    private void BurstPullTrigger(){
        if(currentAmmo > 0 && timeUntilCanShoot <= 0){
            Debug.Log("Shot fired");
            Shoot();
            timeUntilCanShoot = shootDelay;
            burstAmountLeft--;
        }else if(timeUntilCanShoot <= 0){
            burstAmountLeft--;
        }
    }

    [PunRPC]
    private void FlipTrue(){
        renderer.flipX = true;
    }

    [PunRPC]
    private void FlipFalse(){
        renderer.flipX = false;
    }

    public void Shoot(){
        direction.Normalize();

        bullet = PhotonNetwork.Instantiate(bulletPrefabName, bulletStart.position, Quaternion.identity);
        bullet.transform.parent = gameObject.transform;
        bullet.GetComponent<PhotonView>().RPC("SetDirection", RpcTarget.All, direction);

        shootingSource.Play();
        currentAmmo--;
        SetAmmoText();
    }

    void InitialiseWeapon()
    {
        gunPivotPoint = gameObject.transform.Find("GunHolder");
        gun = gunPivotPoint.transform.Find("Gun");
        bulletStart = gun.transform.Find("FirePoint");
    }

    IEnumerator ReloadAmmo(){
        if(currentAmmo != maxAmmo){
            currentAmmo = 0;
            reloadSource.Play();
            ammoText.text = "Reloading";
            yield return new WaitForSeconds(2);
            currentAmmo = maxAmmo;
            SetAmmoText();
        }
    }

    private void SetAmmoText(){
        if(photonView.IsMine){
            ammoText.text = "Ammo: "+currentAmmo+"/"+maxAmmo;
        }
    }

    public void WalkingSound(float horizontal, float vertical){
        if(horizontal != 0 || vertical != 0){
            if(!walkSoundPlaying){
                walkSoundPlaying = true;
                walkingSource.Play();
            }
        }else{
            if(walkSoundPlaying){
                walkSoundPlaying = false;
                walkingSource.Stop();
            }
        }
    }

    public void SetWeapon(int spriteIndex, string newBulletPrefabName, int newMaxAmmo, float newShootDelay, int newBurstAmount, bool isGunAutomatic, bool isGunBurst){
        photonView.RPC("SetWeaponRPC", RpcTarget.AllBuffered, spriteIndex, newBulletPrefabName, newMaxAmmo, newShootDelay, newBurstAmount, isGunAutomatic, isGunBurst);
        SetAmmoText();
    }
    [PunRPC]
    private void SetWeaponRPC(int spriteIndex, string newBulletPrefabName, int newMaxAmmo, float newShootDelay, int newBurstAmount = 0, bool isGunAutomatic = false, bool isGunBurst = false){
        maxAmmo = newMaxAmmo;
        currentAmmo = maxAmmo;
        gun.gameObject.GetComponent<SpriteRenderer>().sprite = weaponSprites[spriteIndex];
        bulletPrefabName = newBulletPrefabName;
        automaticWeapon = isGunAutomatic;
        shootDelay = newShootDelay;


        burstWeapon = isGunBurst;
        burstAmount = newBurstAmount;
    }
}
