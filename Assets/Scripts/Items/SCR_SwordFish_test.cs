using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_SwordFish_test : MonoBehaviour {

    public Transform Target;
    public float firingAngle = 45.0f;
    public float gravity = 9.8f;

    public Transform Projectile;
    private Transform myTransform;
    float projectile_Velocity;
    float target_Distance;
    float flightDuration;
    private bool onlyone = false;


    void Awake()
    {
        myTransform = transform;
    }

    void Start()
    {
        //StartCoroutine(SimulateProjectile());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && onlyone == false) 
        {
            StartCoroutine(SimulateProjectile());
            onlyone = true;
        }
    }
    private void OnCollisionEnter(Collision _col)
    {
        if (_col.gameObject.tag == "Wall" || _col.gameObject.tag == "Ground")
        {
            //hit = true;
            myTransform.gameObject.isStatic = true;
            myTransform.GetComponent<Rigidbody>().velocity = Vector3.zero;
            myTransform.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            myTransform.GetComponent<Rigidbody>().isKinematic = true;
            flightDuration = 0;
            Debug.Log(_col.gameObject.tag);
        }
    }


    IEnumerator SimulateProjectile()
    {
        Projectile.position = myTransform.position + new Vector3(0, 0.0f, 0);
        target_Distance = Vector3.Distance(Projectile.position, Target.position);
        projectile_Velocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad)/gravity);

        float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);

        flightDuration = target_Distance / Vx;

        Projectile.rotation = Quaternion.LookRotation(Target.position - Projectile.position);

        float elapse_time = 0;

        while (elapse_time < flightDuration)
        {
            Projectile.Translate(0, (Vy - (gravity * elapse_time)) * Time.deltaTime, Vx * Time.deltaTime);

            elapse_time += Time.deltaTime;

            yield return null;
        }
    }
}