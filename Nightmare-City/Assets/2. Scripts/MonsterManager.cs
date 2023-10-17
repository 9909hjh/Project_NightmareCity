using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{

    [Tooltip("시간 표시")]
    int prevTime;
    [Tooltip("전체 시간")]
    private float surviveTime;

    [Tooltip("랜덤 생성 범위 설정")]
    public float randomX;
    public float randomZ;
    int prevMonsterCheck;

    private AudioSource aud;

    public GameObject monsterPrefab;
    public AudioClip Music;

    private void Start()
    {
        aud = GetComponent<AudioSource>();
        aud.clip = Music;
        aud.Play();
    }

    void Update()
    {
        CreateMonster();
    }

    void CreateMonster()
    {
        surviveTime += Time.deltaTime;
        
        if(surviveTime % 5f <= 0.01f)
        {
            float MoveX = Random.Range(-randomX, randomX);
            float MoveZ = Random.Range(-randomZ, randomZ);

            Vector3 randompos = new Vector3(transform.position.x + MoveX, transform.position.y, transform.position.z + MoveZ);
            Debug.Log("생성");
            GameObject monster = Instantiate(monsterPrefab, randompos, Quaternion.identity);
            //monster.transform.parent = ;
            monster.transform.localPosition = randompos;
        }
        //prevMonsterCheck = (int)(surviveTime % 5f);
    }
}
