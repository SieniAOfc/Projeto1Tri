using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MouseLook : MonoBehaviour
{
    [SerializeField] float mouseS = 100f; //sensibilidade--faz a camera girar em uma vl "agradavel"
    [SerializeField] Transform playerB; //serve pro corpo do personagem girar junto com a camera
    private float xRotation = 0; //guarda o quanto o player olhou pra cima ou pra baixo
    private InputAction lookAction; //guarda a action look
    public InputActionAsset InputActions;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; //trava o mouse no meio da tela
    }

    void Awake()
    {
        
        lookAction = InputSystem.actions.FindAction("Look"); //procura a acao look e atribui ela a variavel

    }
    void Update()
    {
        
        Vector2 mouseDelta = lookAction.ReadValue<Vector2>(); //captura o movimento do mouse

        float mouseX = mouseDelta.x * mouseS * Time.deltaTime; //calcula o movimento na horizontal e tem deltaTime pra a velocidade ser a mesma independente do pc
        float mouseY = mouseDelta.y * mouseS * Time.deltaTime; //faz o mesmo, mas na vertical

        xRotation -= mouseY; //subtrai o movimento do mouse, pq pra olhar pra cima o valor de x da rotacao precisa diminuir
        xRotation = Mathf.Clamp(xRotation, -90, 90f); //impede que o rotation seja menor que -90 ou maior que 90 pra nao inverter a camera
        transform.localRotation = Quaternion.Euler(xRotation, 0, 0); //aplica a rotacao de cima e baixo só na camera
        playerB.Rotate(Vector3.up * mouseX); //o corpo do player gira em volta de si mesmo baseado no movimento horizontal do mouse

    }
}

//quaternion é pra converter os numeros na linguagem de rotação do computador
