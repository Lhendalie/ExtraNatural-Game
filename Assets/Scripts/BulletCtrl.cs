using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    public float speed = 2.0f;
    public bool facingRight;

    private bool isHit;
    private PlayerCtrl pm;
    private EnemyCtrl ec;

    void Start()
    {
        pm = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCtrl>();

        facingRight = pm.facingRight;
        Destroy(gameObject, 0.6f);
    }

    void Update()
    {
        //Moves the bullet according to direction
        if (facingRight)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        //Takes healthpoints from the ghost if the ghost is hit by bullets and destroys the bullets
        if (col.CompareTag("Ghost"))
        {
            isHit = true;
            if (isHit)
            {
                ec = col.GetComponent<EnemyCtrl>();
                ec.health = ec.health - 2;
                ec.anim.Play("Ghost_1_Hurt");
                isHit = false;
            }
            if(!isHit)
            {
                ec.anim.Play("Ghost_1_Walk");
            }
        }

        if (!col.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
