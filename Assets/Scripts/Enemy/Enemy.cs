using UnityEngine;
using UnityEngine.Pool;

public class Enemy : MonoBehaviour
{
    private GameManager gameManager => GameManager.instance;

    private int uid = 0;

    private float startSpeed;
    private float speed;
    private int damage = 1;
    private float hp = 100;
    private int value = 50;

    private float rotationSpeed = 10f;

    private bool immuneStun;
    private bool immuneSlow;

    private float slowAmount;

    private float slowCount;
    private float stunCount;

    private bool nowSlow;
    private bool nowStun;

    private bool isDead = false;
    public bool IsDead => isDead;

    private IObjectPool<Enemy> objectPool;
    public IObjectPool<Enemy> ObjectPool { set => objectPool = value; }

    private EnemyData enemyData;

    private Transform target;
    private int wavepointIndex = 0;
    private Transform[] waypoints;

    [SerializeField] private GameObject slowEffect;
    [SerializeField] private GameObject stunEffect;

    void FixedUpdate()
    {
        if (isDead)
            return;

        if(nowStun)
        {
            stunCount -= Time.fixedDeltaTime;
            if (stunCount <= 0)
            {
                nowStun = false;
                stunEffect.SetActive(false);
            }
            else
                return;
        }

        if (nowSlow)
        {
            slowCount -= Time.fixedDeltaTime;
            if (slowCount <= 0)
            {
                nowSlow = false;
                slowEffect.SetActive(false);
                speed = startSpeed;
            }
        }
        
        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * speed * Time.fixedDeltaTime, Space.World);

        if (dir != Vector3.zero)
        {
            Quaternion wantedRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, wantedRot, rotationSpeed * Time.fixedDeltaTime);
        }

        if (Vector3.Distance(transform.position, target.position) <= 0.25f)
        {
            GetNextWaypoint();
        }
    }

    public void SetEnemy(EnemyData enemyData)
    {
        this.enemyData = enemyData;
        hp = enemyData.HP;
        value = enemyData.Value;
        
        startSpeed = enemyData.Speed;
        speed = enemyData.Speed;
        
        immuneSlow = enemyData.ImmuneSlow;
        immuneStun= enemyData.ImmuneStun;

        damage = enemyData.Damage;
    }

    public void ReSwpawn(Transform[] waypoints,int uid)
    {
        hp = enemyData.HP;
        speed = startSpeed;
        this.waypoints = waypoints;
        target = waypoints[0];
        wavepointIndex = 0;
        isDead = false;
        this.uid = uid;

        nowStun = false;
        nowSlow = false;
        stunEffect.SetActive(false);
        slowEffect.SetActive(false);

        stunCount = 0;
        slowCount = 0;

        slowAmount = 0;
    }

    public void TakeDamage(float damage,float debuffAmount = 0f,float debuffDuration = 0f,DebuffType debuff = DebuffType.None)
    {
        if (isDead) return;

        hp -= damage;
        if (hp <= 0)
        {
            Die();
            return;
        }

        if(debuff == DebuffType.Stun && immuneStun == false)
        {
            if(Random.Range(0f,1f) <= debuffAmount)
            {
                nowStun = true;
                //剩餘時間大於要附上的時間，維持不變
                stunCount = stunCount > debuffDuration ? stunCount : debuffDuration;
               stunEffect.SetActive(true);
            }
        }
        else if(debuff == DebuffType.Slow && immuneSlow == false)
        {
            if (slowAmount <= debuffAmount)
            {
                nowSlow = true;
                slowAmount = debuffAmount;
                slowCount = debuffDuration;
                speed = startSpeed * (1f - slowAmount);
                slowEffect.SetActive(true);
            }
        }

    }

    private void Die()
    {
        isDead = true;
        EventHelper.EffectShowEvent.Invoke(this,GameEvent.GameEffectShowEvent.CreateEvent(transform.position,enemyData.DeadParticle));

        //發送死亡事件
        EventHelper.EnemyDiedEvent.Invoke(this,GameEvent.EnemyDieEvent.CreateEvent(uid,value));
        objectPool.Release(this);
    }

    private void GetNextWaypoint()
    {
        if (wavepointIndex >= waypoints.Length - 1)
        {
            EndPath();
            return;
        }

        wavepointIndex++;
        target = waypoints[wavepointIndex];
    }

    private void EndPath()
    {
        objectPool.Release(this);
        EventHelper.EnemyEndPathEvent.Invoke(this, GameEvent.EnemyEndPathEvent.CreateEvent(uid,damage));
    }
}
