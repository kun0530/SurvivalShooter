using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;
using UnityEngine.Pool;

public class Enemy : LivingEntity
{
    // 추적
    public LayerMask whatIsTarget;
    private LivingEntity targetEntity;
    private NavMeshAgent pathFinder;

    // 피격
    public ParticleSystem hitEffects;
    public int score = 10;

    // 공격
    public float damage = 20f;
    public float attackInterval = 0.5f;
    private float lastAttackTime;

    // 애니메이션
    private Animator enemyAnimator;

    // 오브젝트 풀
    public IObjectPool<Enemy> pool;

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

    protected override void OnEnable()
    {
        base.OnEnable();
        
        pathFinder.enabled = true;
        var cols = GetComponentsInChildren<Collider>();
        foreach(Collider col in cols)
        {
            col.enabled = true;
        }
        
        lastAttackTime = Time.time;
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

    private void OnTriggerStay(Collider other)
    {
        if (!isDead && Time.time > lastAttackTime + attackInterval)
        {
            var livingEntity = other.GetComponent<LivingEntity>();
            if (livingEntity != null && livingEntity == targetEntity)
            {
                var pos = transform.position;
                pos.y += 1;
                var hitPoint = other.ClosestPoint(pos);
                var hitNomal = other.transform.position - targetEntity.transform.position;
                livingEntity.OnDamage(damage, hitPoint, hitNomal.normalized);

                lastAttackTime = Time.time;
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

    public override void OnDie()
    {
        pathFinder.isStopped = true;
        pathFinder.enabled = false;

        enemyAnimator.SetTrigger("Die");
        // Debug.Log("Death Animation: " + ", called at: " + Time.time);

        base.OnDie();

        GameManager.Instance.Score += score;
    }
    
    private void StartSinking()
    {
        var cols = GetComponentsInChildren<Collider>();
        foreach(Collider col in cols)
        {
            col.enabled = false;
        }
        StartCoroutine(CoSinking(1f));
    }

    private IEnumerator CoSinking(float duration)
    {
        float timer = 0f;
        while (duration > timer)
        {
            timer += Time.deltaTime;
            transform.position -= new Vector3(0f, 3f * Time.deltaTime, 0f);
            yield return null;
        }

        // 비활성화 후 오브젝트 풀에 집어넣는다.
        if (pool != null) 
        {
            pool.Release(this);
        }
    }
}