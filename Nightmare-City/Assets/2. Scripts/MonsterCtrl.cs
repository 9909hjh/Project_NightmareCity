using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class MonsterCtrl : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    private Animator ani;
    private Animator BossAni;
    private AudioSource audioSource;

    //HP
    public float health;

    // Patroling
    public Vector3 walkPoint;
    public bool walkPointSet = false;
    public float walkPointRange;
    
    // Move
    public int MonsterMoveX = 0;
    public int MonsterMoveZ;
    private int MoveCheck = 0;
    public bool MonsterDir = false;
    public bool MonsterMoveCheck = false;

    // Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;

    // State
    public float sightRange, attackRange; // eyeContactRange
    public bool playerInSightRange, playerInAttackRange;
    public float sightRangecharge;

    // Monster Sortation
    public bool Monster = false;
    public bool Boss = false;

    // Effect
    public GameObject bloodEffect;

    // Die Check
    public bool MonsterDie = false;
    public bool MonsterDieCheck = false;

    // Sound
    public AudioClip MonsterIdle;
    public AudioClip MonsterWalk;
    public AudioClip MonsterDied;
    public AudioClip BossWav;
    public AudioClip BossDie;
    public bool ZonbieSounds = true;
    public bool BossSounds = true;

    Rigidbody rigid;
    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();

        if (Monster)
        {
            ani = GetComponent<Animator>();
        }
        if(Boss)
        {
            BossAni = GetComponent<Animator>();
            rigid = GetComponent<Rigidbody>();
        }
    }

    private void Update()
    {
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (Monster)
        {
            if (!playerInSightRange && !playerInAttackRange)
            {
                Patroling();
                MonsterMoveCheck = false;
            }
            if (ZonbieSounds)
            {
                MonsterSound("Idle");
                ZonbieSounds = false;
            }
        }

        if (playerInSightRange && !playerInAttackRange)
        {
            ChasePlayer();
            
            if (Monster)
            {
                MonsterMoveCheck = true;
                sightRange = sightRangecharge;
                MonsterAni("IsWalkTrue");
            }
            if(Boss)
            {
                if (BossSounds)
                {
                    BossSound("Wav");
                    Debug.Log("Boss사운드");
                    BossSounds = false;
                }
            }
        }

        if (playerInAttackRange && playerInSightRange)
        {
            AttackPlayer();
            if (Monster)
            {
                MonsterAni("IsAttackTrue");
            }
            if(Boss)
            {
                BossAnimator("IsAttackTrue");
            }
        }

        if(health <= 0)
        {
            if(Boss)
            {
                Img.SetActive(true);
                StartCoroutine(ifBossDie());
            }
        }
    }
    public GameObject Img;
    public UI_Fade Fade;
    public IEnumerator ifBossDie()
    {
        yield return new WaitForSeconds(1.5f);
        Fade.FadeIn();
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene("clear");
    }

    // Freedom Move
    private void Patroling()
    {
       
        MonsterAni("IsWalkFalse");
        

        if (!walkPointSet)
        {
            SearchWalkPoint();
            MonsterAni("IsTraceFalse"); 
        }

        if(walkPointSet)
        {
            agent.SetDestination(walkPoint);
            //MonsterSound("Walk");
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        
        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
            if (MoveCheck == 0)
            {
                MonsterDir = true;
                MoveCheck = 1;
            }
            else if (MoveCheck == 1)
            {
                MonsterDir = false;
                MoveCheck = 0;
            }
        }
    }
    // Move X,Z
    private void SearchWalkPoint()
    {
        int MoveX;
        int MoveZ;

        // Calculate random point in range
        if (!MonsterDir)
        {
            MoveX = MonsterMoveX;
            MoveZ = MonsterMoveZ;
        }
        else
        {
            MoveX = -MonsterMoveX;
            MoveZ = -MonsterMoveZ;
        }
        

        walkPoint = new Vector3(transform.position.x + MoveX, transform.position.y, transform.position.z + MoveZ);

        
        // 지상에 있는지 확인
        Invoke("WalkPointSetting", 4f);
  
    }
    // Move On
    private void WalkPointSetting()
    {
       walkPointSet = true;
        if(MonsterMoveCheck || MonsterMoveX != 0 || MonsterMoveZ != 0)
        {
            MonsterAni("IsTraceTrue");
        }
    }
    

    // Player Followed
    private void ChasePlayer()
    {
        if (!MonsterDie)
        {
            agent.SetDestination(player.position);
            MonsterSound("Walk");
        }
    }

    // Player Attect
    private void AttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if(!alreadyAttacked)
        {
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    public void MonAttack()
    {
        projectile.SetActive(true);
    }

    public void AttackDestroy()
    {
        projectile.SetActive(false);
    }

    // Attack Cool time
    private void ResetAttack()
    {
        alreadyAttacked = false;
        if (Monster)
        {
            MonsterAni("IsAttackFalse");
        }
        if (Boss)
        {
            BossAnimator("IsAttackFalse");
        }
    }

    public UI_BossHP hpBar;
    public void TakeDamage(int damage)
    {
        health -= damage;
        if(Boss)
        {
            hpBar.SetHP(health);
        }

        if (health <= 0)
        {
            MonsterDieCheck = true;
            if (Monster)
            {
                MonsterAni("IsDeadTrue");
                MonsterDie = true;
                ZonbieSounds = true;
                gameObject.tag = "DieEnemy";
                if (ZonbieSounds)
                {
                    MonsterSound("Die");
                    ZonbieSounds = false;
                }
            }
            if (Boss)
            {
                BossAnimator("IsDeadTrue");
                BossSounds = true;
                gameObject.tag = "DieEnemy";
                if (BossSounds)
                {
                    BossSound("Die");
                    BossSounds = false;
                }
            }
            Invoke(nameof(DestroyEnemy), 0.7f);
        }
    }

    private void DestroyEnemy()
    {
        if(Monster)
        {
            Destroy(gameObject);
        }
    }

    // Zombie Animation
    private void MonsterAni(string Ani)
    {
        switch (Ani)
        {
            case "IsTraceTrue":
                ani.SetBool("IsTrace", true);
                break;
            case "IsTraceFalse":
                ani.SetBool("IsTrace", false);
                break;
            case "IsWalkTrue":
                ani.SetBool("IsWalk", true);
                break;
            case "IsWalkFalse":
                ani.SetBool("IsWalk", false);
                break;
            case "IsAttackTrue":
                ani.SetBool("IsAttack", true);
                break;
            case "IsAttackFalse":
                ani.SetBool("IsAttack", false);
                break;
            case "IsDeadTrue":
                ani.SetTrigger("IsDead");
                break;
            default:
                break;
        }
    }

    // Boss Animation
    private void BossAnimator(string Ani)
    {
        switch (Ani)
        {
            case "IsAttackTrue":
                BossAni.SetBool("IsAttack", true);
                break;
            case "IsAttackFalse":
                BossAni.SetBool("IsAttack", false);
                break;
            case "IsDeadTrue":
                BossAni.SetTrigger("IsDead");
                break;
            default:
                break;
        }
    }

    // Monster Sound
    private void MonsterSound(string action)
    {
        switch(action)
        {
            case "Idle":
                audioSource.clip = MonsterIdle;
                break;
            case "Walk":
                audioSource.clip = MonsterWalk;
                break;
            case "Die":
                audioSource.clip = MonsterDied;
                break;
            default:
                Debug.Log("ERROR : 해당 사운드 값 없음");
                break;
        }
        audioSource.Play();
    }
    // Boss Sound
    private void BossSound(string action)
    {
        switch (action)
        {
            case "Wav":
                audioSource.clip = BossWav;
                break;
            case "Die":
                audioSource.clip = BossDie;
                break;
            default:
                Debug.Log("ERROR : 해당 사운드 값 없음");
                break;
        }
        audioSource.Play();
    }

    // Particle
    public void CreateBloodEffect()
    {
        // 혈흔 생성
        if (Monster)
        {
            Vector3 pos = transform.position + transform.up * 5f;
            GameObject blood1 = (GameObject)Instantiate(bloodEffect, pos, Quaternion.identity);
            Destroy(blood1, 1.0f);
        }
        else if (Boss)
        {
            Vector3 pos = transform.position + transform.up * 7f;
            GameObject blood1 = (GameObject)Instantiate(bloodEffect, pos, Quaternion.identity);
            Destroy(blood1, 1.0f);
        }

    }

    // Player Check Range 
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
