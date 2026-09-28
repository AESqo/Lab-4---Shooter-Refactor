using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;

public class Enemy : MonoBehaviour
{
    public Transform player;
    public float orbitRadius = 5f;
    public float orbitSpeed = 2f;
    private float currentAngle = 0f;
    private Vector3 orbitPoint;

    public float baseSpeed = 1f;
    public float maxSpeed = 2f;
    public float squareRange = 128f;

    private CinemachineImpulseSource impulseSource;

    void Start() {
        orbitPoint = player.position;
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }
    void Update()
    {
        if (player == null) return;
        Vector3 toPlayer = player.position - transform.position;
        float squareDistance = toPlayer.sqrMagnitude;
        float currentSpeed = baseSpeed;
        if (squareDistance < squareRange) {
            float distanceFactor = 1f - (squareDistance / squareRange);
            distanceFactor = distanceFactor * distanceFactor;
            currentSpeed = Mathf.Lerp(baseSpeed, maxSpeed, distanceFactor);
        }
        currentAngle += currentSpeed * Time.deltaTime;
        float x = Mathf.Cos(currentAngle) * orbitRadius;
        float y = Mathf.Sin(currentAngle) * orbitRadius;
        transform.position = orbitPoint + new Vector3(x, y, 0f);

        float angleTargetRadius = Mathf.Atan2(toPlayer.y, toPlayer.x);
        float angleTargetDegrees = angleTargetRadius * Mathf.Rad2Deg;
        Quaternion rotationTarget = Quaternion.Euler(0, 0, angleTargetDegrees - 90f);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotationTarget, 5f * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D whatIHit)
    {
        if (whatIHit.tag == "Player")
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().gameOver = true;
            Destroy(whatIHit.gameObject);
            Destroy(this.gameObject);
        } else if (whatIHit.tag == "Laser")
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().meteorCount++;
            impulseSource.GenerateImpulse();
            Destroy(whatIHit.gameObject);
            Destroy(this.gameObject);
        }
    }
}
