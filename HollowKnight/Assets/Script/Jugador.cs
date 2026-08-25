using UnityEngine;
using UnityEngine.InputSystem;

public class Jugador : MonoBehaviour
{
    CharacterController characterController;
    PlayerInput playerInput;
    Vector3 velocidad;
    Vector3 rotacion;
    Animator animator;
    int saltos_restantes;
    float is_graunded;

    //Estados
    float contadorDash;
    float cooldawnDash;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterController = this.GetComponent<CharacterController>();
        playerInput = this.GetComponent<PlayerInput>();
        velocidad = Vector3.zero;
        rotacion = Vector3.zero;
        animator = this.transform.GetChild(0).GetComponent<Animator>();

       // Application.targetFrameRate = 1;

        saltos_restantes = 1;
        is_graunded = 0;

        //Estados
        contadorDash = 0;
        cooldawnDash = 0;
    }

    // Update is called once per frame
    void Update()
    {
        velocidad.y -= 60 * Time.deltaTime;

        //playerInput.actions["Move"].ReadValue<Vector2>();
        if (contadorDash <= 0)
        {
            //velocidad.x = 16;
            velocidad.x = playerInput.actions["Move"].ReadValue<Vector2>().x * 4;
        }
        else
        {
            velocidad.y = 0;
        }

        cooldawnDash -= Time.deltaTime;
        if (playerInput.actions["Sprint"].WasPressedThisFrame())
        {
            if(contadorDash<=0)
            {
                contadorDash = .2f;
                if (rotacion.y == 0)
                {
                    velocidad.x = 20;
                }
                else
                {
                    velocidad.x = -20;
                }
                cooldawnDash = 0.3f;
            }
            
        }
        contadorDash-= Time.deltaTime;

        /*if(contadorDash>0)
        {
            velocidad.x = 16;
        }
        //1;*/
        if (velocidad.x > 0)
        {
            rotacion.y = 0;
        }
        if (velocidad.x < 0)
        {
            rotacion.y = 180;
        }

        is_graunded -= Time.deltaTime;
        
        if(characterController.isGrounded)
        {
            is_graunded = 0.3f;
        }

        if (is_graunded>0)
        {
            saltos_restantes = 1;
            if (velocidad.x == 0)
            {
                animator.Play("JugadorIddle");
            }
            else
            {
                animator.Play("Jugador_Caminando");
            }
                velocidad.y = -1;
            if (playerInput.actions["Jump"].WasPressedThisFrame() )
            {

                velocidad.y = 14;
                is_graunded = 0;
                animator.Play("Jugador_Saltar");
            }
        }
        else
        {
            if (playerInput.actions["jump"].IsPressed())
            {
                velocidad.y += 40 * Time.deltaTime;
            }
            

            if (velocidad.y < -1)
            {
                animator.Play("Jugador_Caida");
            }
            else if (velocidad.y < 0)
            {
                animator.Play("Jugador_Empezar_Caida");
            }
            if (playerInput.actions["Jump"].WasPressedThisFrame())
            {
                if (saltos_restantes > 0)
                {
                    velocidad.y = 14;
                    saltos_restantes--;
                    animator.Play("Jugador_DobleSalto");
                }
            }
        }
        characterController.Move(velocidad * Time.deltaTime);
        this.transform.rotation = Quaternion.Euler(rotacion);
        //characterController.Move(Vector3.down);
        //Que son lasl corrutinas

        Debug.DrawRay(this.transform.position + Vector3.up * 0.5f, Vector3.up* 0.7f, Color.purple);
        RaycastHit hit;
        if(Physics.Raycast(this.transform.position + Vector3.up * 0.5f, Vector3.up, out hit, 0.8f))
        {
            velocidad.y = -4;
        }


    }
}
