﻿using UnityEngine;
using System.Collections;

public class CharacterMovement : MonoBehaviour
{

    public float maxSpeed = 2.0f;
    public bool facingRight = true;
    public float moveSpeed;

    public bool doubleJump = false;
    public int maxJumpCount = 5;
    public int jumpCount = 0;
    public float jumpSpeed = 200;
    private Rigidbody rigidbody;
    public Transform groundCheck;
    public float groundRadius = 0.001f;
    public LayerMask whatIsGround;
    public bool grounded = false;

    public float turnWaitTime = .2f;
    public float turnTime = 0;
    public bool turning = false;


    public float swordSpeed = 600.0f;
    public Transform shotSpawn;
    public Rigidbody swordPrefab;

    private Animator anim;

    Rigidbody clone;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        // Para que el rigidbody no deje de recibir eventos mientras esta inmovil
        rigidbody.sleepThreshold = 0.0f;

        // Para comprobar cuando se toca el suelo
        groundCheck = GameObject.Find("/Character/GroundCheck").transform;
        // Punto desde donde salen los disparos
        shotSpawn = GameObject.Find("ShotSpawn").transform;
        // Objeto que manipula las animaciones
        anim = GameObject.Find("/Character/CharacterSprite").GetComponent<Animator>();
        // Contiene todas las partes del caracter excepto el Sprite
    }

    void FixedUpdate()
    {
        // Se chekea si se toca el suelo y se actualizan los estados de la animacion
        grounded =
            Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
        anim.SetBool("Grounded", grounded);
        anim.SetFloat("MoveSpeed", moveSpeed);

        // Si se esta sobre el suelo se inicializan los estados de salto
        if (grounded)
        {
            doubleJump = false;
            jumpCount = 0;
        }

        // En funcion del movimiento se cambia la orientacion del personaje
        if (moveSpeed > 0.0f && !facingRight)
        {
            Flip();
        }
        else if (moveSpeed < 0.0f && facingRight)
        {
            Flip();
        }

        // Mientras ocurre el giro del personaje no ocurre movimiento
        if (!turning)
        {
            anim.SetBool("Turning", turning);
            rigidbody.velocity =
            new Vector2(moveSpeed * maxSpeed, rigidbody.velocity.y);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Se captura la direccion y velocidad del movimiento horizontal
        moveSpeed = Input.GetAxis("Horizontal");

        // Si el personaje esta en el suelo o si kedan saltos permisibles aun y se presiona saltar
        if ((grounded || jumpCount < maxJumpCount) && Input.GetButtonDown("Jump"))
        {
            // Se annade una fuerza vertical (Salto)
            rigidbody.AddForce(new Vector2(0.0f, jumpSpeed));
            // Se actualiza la cantidad de saltos
            if (jumpCount < maxJumpCount && !grounded) jumpCount++;
        }

        // Se actualiza el estado del giro
        if (turning)
        {
            turnTime += Time.deltaTime;
            if (turnTime >= turnWaitTime)
            {
                turning = false;
                turnTime = 0;
            }
        }

        if (Input.GetButtonDown("Fire1"))
        {
            Attack();
        }
    }

    void Flip()
    {
        // Gira el pesonaje 180 grados y actualiza el estado de las animaciones
        turning = true;
        transform.Rotate(Vector3.up, 180.0f, Space.World);
        // Se ignora el giro para el CharacterSprite porque la animacion es asimetrica
        anim.transform.Rotate(Vector3.up, 180.0f, Space.World);

        facingRight = !facingRight;
        anim.SetBool("FacingRight", facingRight);
        anim.SetBool("Turning", turning);
    }
    void Attack()
    {
        //clone =
        //    Instantiate(swordPrefab, shotSpawn.position, shotSpawn.rotation) as Rigidbody;
        //clone.AddForce(shotSpawn.transform.right * swordSpeed);
    }
}