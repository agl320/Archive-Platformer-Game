using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Impact : MonoBehaviour
{

    [SerializeField] public GameObject impact;
    [SerializeField] public GameObject fragment;

    GameObject particle, fragmentParticle;

    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        
    }

    // Update is called once per frame
    void Update()
    {


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Weapon")
        {
            CreateFragmentParticle();
            CreateImpactParticle();
        }
    }

    private void CreateImpactParticle()
    {
        particle = Instantiate(impact,
            transform.position, Quaternion.identity) as GameObject;

        ParticleSystem.EmitParams velocityParam = new ParticleSystem.EmitParams();
        velocityParam.velocity = new Vector2(rb.velocity.x, rb.velocity.y);

        particle.GetComponent<ParticleSystem>().Emit(velocityParam, 5);

        particle.GetComponent<ParticleSystem>().Stop();

        Destroy(particle, 10);
    }

    private void CreateFragmentParticle()
    {
        particle = Instantiate(fragment,
            transform.position, Quaternion.identity) as GameObject;

        ParticleSystem.EmitParams velocityParam = new ParticleSystem.EmitParams();
        velocityParam.velocity = new Vector2(rb.velocity.x, rb.velocity.y);

        particle.GetComponent<ParticleSystem>().Emit(velocityParam, 10);

        particle.GetComponent<ParticleSystem>().Stop();

        Destroy(particle, 10);
    }
    
}
