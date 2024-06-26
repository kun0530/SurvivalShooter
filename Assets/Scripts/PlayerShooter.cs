using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    private PlayerInput playerInput;
    public Transform fireTransform;
    public LineRenderer bulletLineRenderer;
    public ParticleSystem gunParticles;
    public AudioSource gunAudioPlayer;
    public AudioClip shotClip;
    private float fireDistance = 50f;
    public float fireInterval = 1f;
    private float lastFireTime;
    public float damage = 25f;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        bulletLineRenderer.enabled = false;
        bulletLineRenderer.positionCount = 2;
    }

    private void OnEnable()
    {
        lastFireTime = 0f;
    }

    void Update()
    {
        if (GameManager.Instance.IsPaused)
            return;

        // 입력을 감지하고 총 발사하거나 재장전
        if (playerInput.fire)
        {
            Fire();
        }
    }

    // 발사 시도
    public void Fire()
    {
        if (Time.time > lastFireTime + fireInterval)
        {
            lastFireTime = Time.time;
            Shot();
        }
    }

    // 발사
    private void Shot()
    {
        var hitPoint = Vector3.zero;
        var ray = new Ray(fireTransform.position, fireTransform.forward);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, fireDistance))
        {
            hitPoint = hitInfo.point;

            var damageable = hitInfo.collider.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.OnDamage(damage, hitPoint, hitInfo.normal);
            }
        }
        else
        {
            hitPoint = fireTransform.position + fireTransform.forward * fireDistance;
        }

        StartCoroutine(ShotEffect(hitPoint));
    }

    private IEnumerator ShotEffect(Vector3 hitPosition)
    {
        // 라인 렌더러를 활성화하여 총알 궤적을 그린다
        bulletLineRenderer.enabled = true;

        bulletLineRenderer.SetPosition(0, fireTransform.position);
        bulletLineRenderer.SetPosition(1, hitPosition);

        gunParticles.transform.position = fireTransform.position;
        gunParticles.transform.rotation = Quaternion.LookRotation(fireTransform.forward);
        gunParticles.Play();
        gunAudioPlayer.PlayOneShot(shotClip);

        // 0.03초 동안 잠시 처리를 대기
        yield return new WaitForSeconds(0.03f);

        // 라인 렌더러를 비활성화하여 총알 궤적을 지운다
        bulletLineRenderer.enabled = false;
    }
}
