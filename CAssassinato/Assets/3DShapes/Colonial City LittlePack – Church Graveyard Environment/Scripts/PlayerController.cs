using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    [SerializeField] CharacterController controller;
    [SerializeField] float speed = 1.5f;
    [SerializeField] float gravity= -10;
    private InputAction moveAction;
    private Vector3 velocity; //guarda a velocidade de queda
    [SerializeField] Transform groundC;
    private float groundD = 0.4f;
    [SerializeField] LayerMask groundM;
    private bool isGrounded;

    void Awake()
    {
        
        moveAction = InputSystem.actions.FindAction("Move"); //procura a acao move e atribui ela a variavel

    }

    void Update()
    {
        
        Vector2 input = moveAction.ReadValue<Vector2>(); 

        Vector3 move = transform.right * input.x + transform.forward * input.y; //garante que o personbagem move para onde ele esta olhando

        controller.Move(move * speed * Time.deltaTime); //faz o personagem andar

        velocity.y += gravity * Time.deltaTime; //garante que o jogador nao saia voando pelo mapa

        controller.Move(velocity * Time.deltaTime); //faz com que o corpo sempre esteja no chao

        isGrounded = Physics.CheckSphere(groundC.position, groundD, groundM);
        if(isGrounded && velocity.y < 0) //se estiver no chao e a velocidade no y for menor que 0, entao a vy ganha -2f, para fazer que a velocidade de queda nao seja constante mesmo nao estando em queda
        {
            
            velocity.y = -2f;

        }

    }
}
