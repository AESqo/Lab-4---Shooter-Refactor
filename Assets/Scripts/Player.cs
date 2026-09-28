using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public GameObject laserPrefab;

    private float speed = 6f;
    private float horizontalScreenLimit = 10f;
    private float verticalScreenLimit = 6f;
    private bool canShoot = true;
    private InputSystem_Actions _inputActions;

    // Start is called before the first frame update
    void OnEnable()
    {
        _inputActions = new InputSystem_Actions();
        _inputActions.Player.Enable();
    }
    void OnDisable()
    {
        _inputActions.Player.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Shooting();
    }

    void Movement()
    {
        Vector2 moveInput = _inputActions.Player.Movement.ReadValue<Vector2>();
         Vector3 direction = new Vector3(moveInput.x, moveInput.y, 0);
          transform.Translate(direction * Time.deltaTime * speed);
        if (transform.position.x > horizontalScreenLimit || transform.position.x <= -horizontalScreenLimit)
        {
            transform.position = new Vector3(transform.position.x * -1f, transform.position.y, 0);
        }
        if (transform.position.y > verticalScreenLimit || transform.position.y <= -verticalScreenLimit)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y * -1, 0);
        }

    }

    void Shooting()
    {
        if (_inputActions.Player.Shoot.WasPressedThisFrame() && canShoot)
        {
            Instantiate(laserPrefab, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
            canShoot = false;
            StartCoroutine("Cooldown");
        }
    }

    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(1f);
        canShoot = true;
    }
}
