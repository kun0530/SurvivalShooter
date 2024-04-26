using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
public class Enemy : LivingEntity
{
    // 추적
    public LayerMask whatIsTarget;
    private LivingEntity targetEntity;
    private NavMeshAgent pathFinder;

    // 피격
    public ParticleSystem hitEffects;

    // 애니메이션
    private Animator enemyAnimator;

    private bool hasTarget
    {
        get
        {
            if (targetEntity != null && !targetEntity.isDead)
                return true;
            return false;
        }
    }

    private void Awake()
    {
        pathFinder = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponent<Animator>();
    }

    private void Start()
    {
        // StartCoroutine(UpdatePath());
    }

    private void Update()
    {
        if (isDead)
            return;

        enemyAnimator.SetBool("HasTarget", hasTarget);
    }

    private void FixedUpdate()
    {
        if (isDead)
            return;

        if (hasTarget)
        {
            pathFinder.isStopped = false;
            pathFinder.SetDestination(targetEntity.transform.position);
        }
        else
        {
            pathFinder.isStopped = true;
            Collider[] colliders = Physics.OverlapSphere(transform.position, 100f, whatIsTarget);
            foreach (Collider collider in colliders)
            {
                var livingEntity = collider.GetComponent<LivingEntity>();
                if (livingEntity != null)
                {
                    targetEntity = livingEntity;
                    break;
                }
            }
        }
    }

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        hitEffects.transform.position = hitPoint;
        hitEffects.transform.rotation = Quaternion.LookRotation(hitNormal);
        hitEffects.Play();

        base.OnDamage(damage, hitPoint, hitNormal);
    }

    public override void Die()
    {
        pathFinder.isStopped = true;
        pathFinder.enabled = false;

        enemyAnimator.SetTrigger("Die");
        // Debug.Log("Death Animation: " + ", called at: " + Time.time);

        base.Die();
    }
    
    private void StartSinking(string s)
    {
        // Debug.Log("PrintEvent: " + s + ", called at: " + Time.time);
        var cols = GetComponentsInChildren<Collider>();
        foreach(Collider col in cols)
        {
            col.enabled = false;
        }
        StartCoroutine(Sinking(1f));
    }

    private IEnumerator Sinking(float duration)
    {
        float timer = 0f;
        while (duration > timer)
        {
            timer += Time.deltaTime;
            transform.position -= new Vector3(0f, 3f * Time.deltaTime, 0f);
            yield return null;
        }

        // 비활성화 후 오브젝트 풀에 집어넣는다.
    }
}