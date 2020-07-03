using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCtrl : MonoBehaviour {

    public int health = 10;

    public Animator anim;
    public PlatformMovement pm;
    public PlayerCtrl pc;
    public GameCtrl gc;
    public GameObject panel7;

    private static System.Random rnd = new System.Random();

    void Start () {
        anim = gameObject.GetComponent<Animator>();
        pm = gameObject.GetComponent<PlatformMovement>();
        pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCtrl>();
        gc = GameObject.FindGameObjectWithTag("GameControl").GetComponent<GameCtrl>();

    }

    void Update()
    {
        // Hides other storytelling panels if key panel opens
        if (panel7.activeInHierarchy)
        {
            StartCoroutine(LateCall());
            pc.panel1.SetActive(false);
            pc.panel2.SetActive(false);
            pc.panel3.SetActive(false);
            pc.panel4.SetActive(false);
            pc.panel5.SetActive(false);
            pc.panel6.SetActive(false);
        }
    }

    //If the ghost is dead, plays ghostdead animation and has 20% chance to drop a key
    void GhostDie()
    {
        anim.SetBool("IsDead", true);
        Destroy(pm);
        Destroy(gameObject, 1f);
        pc.enemiesKilled += 1;

        int percent = rnd.Next(0, 100);

        if (!gc.hasKey)
        {
            if (pc.enemiesKilled >= 7 || percent <= 10)
            {
                gc.hasKey = true;
                Debug.Log("Key dropped");
                panel7.SetActive(true);                
                Debug.Log("percent is" + percent);
            }
        }
    }

    IEnumerator LateCall()
    {
        yield return new WaitForSeconds(4);

        panel7.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        //Checks if ghost is attacked by bullets and if so takes health from the ghost. If the ghost's health is zero it kills the ghost
        if (col.CompareTag("bullet"))
        {
            if (health <= 0)
            {
                GameObject.FindGameObjectWithTag("ghostAudio").GetComponent<AudioSource>().volume = 0.04f;
                GameObject.FindGameObjectWithTag("ghostAudio").GetComponent<AudioSource>().Play();
                GhostDie();
            }
        }
    }
}
