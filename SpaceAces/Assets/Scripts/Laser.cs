using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private Transform LaserSpawnPoint;
    [SerializeField]
    private TrailRenderer LaserTrail;
    [SerializeField]
    private float ShootDelay = 0.5f;
    [SerializeField]
    private LayerMask Mask;
    [SerializeField]
    private float LastShootTime;
    [SerializeField]
    public float Damage = 10f;
    [SerializeField]
    private AudioSource LaserSound;
    
    public void Start()
    {
        LaserSound = GetComponent<AudioSource>();
        if (LaserSound == null)
            Debug.LogError("ERORR");
    }

    public void Shoot()
    {
        // Debug.Log("Shoot called in laser");
        if (LastShootTime + ShootDelay < Time.time)
        {
            Vector3 direction = GetDirection();
           
            LastShootTime = Time.time;
            if (Physics.Raycast(LaserSpawnPoint.position, direction, out RaycastHit Hit, float.MaxValue, Mask))
            {
                TrailRenderer trail = Instantiate(LaserTrail, LaserSpawnPoint.position, Quaternion.identity);
                StartCoroutine(SpawnTrail(trail, Hit.point));
                LaserSound.Play();
                
                if (Hit.transform.GetComponent<Target>() != null)
                {
                    Target target = Hit.transform.GetComponent<Target>();
                    target.TakeDamage(Damage);
                }
            }
            else
            {
                Vector3 miss = LaserSpawnPoint.position;
                miss[2] = 100f;
                TrailRenderer trail = Instantiate(LaserTrail, LaserSpawnPoint.position, Quaternion.identity);
                StartCoroutine(SpawnTrail(trail, miss));
                LaserSound.Play();
            }

        }
    }

    private Vector3 GetDirection()
    {
        Vector3 direction = transform.forward;
        return direction;
    }
    // visual beam, create new Trail entity in project 
    private IEnumerator SpawnTrail(TrailRenderer Trail, Vector3 end)
    {
        float time = 0;
        Vector3 startPosition = Trail.transform.position;
        while (time < 1)
        {
            Trail.transform.position = Vector3.Lerp(startPosition, end, time);
            time += Time.deltaTime / Trail.time;
            yield return null;
        }
    
        Trail.transform.position = end;
        //TBD
        //Instantiate(ImpactParticleSystem, Hit.point, Quaternion.LookRotation(Hit.normal));
        Destroy(Trail.gameObject, Trail.time);
    }
}
