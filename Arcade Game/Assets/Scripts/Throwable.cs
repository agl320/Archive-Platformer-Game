using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{

    [SerializeField] public GameObject explosionPrefab;
    [SerializeField] public GameObject particleBurst;
    GameObject particle;
    GameObject explosion;

    float finalScale = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Explode());
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator Explode()
    {
        yield return new WaitForSeconds(2);

        particle = Instantiate(particleBurst,
            transform.position, Quaternion.identity) as GameObject;

        particle.GetComponent<ParticleSystem>().Emit(100);

        particle.GetComponent<ParticleSystem>().Stop();

        Destroy(particle, 10);
        
        explosion = Instantiate(explosionPrefab,
            transform.position,
            Quaternion.identity) as GameObject;
        Destroy(gameObject);
        
        Destroy(this.explosion,0.3f);
        

    }
}
