using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Enemy : LivingEntity
{
    // 추적
    public LayerMask whatIsTarget;
    private LivingEntity targetEntity;
    private NavMeshAgent pathFinder;

    // 피격
    public ParticleSystem hitEffect;

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
        hitEffect.transform.position = hitPoint;
        hitEffect.transform.rotation = Quaternion.LookRotation(hitNormal);
        hitEffect.Play();

        base.OnDamage(damage, hitPoint, hitNormal);
        Debug.Log(currentHealth);
    }
}