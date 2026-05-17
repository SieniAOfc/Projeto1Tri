using UnityEngine;

public class livro : MonoBehaviour
{
    public Transform mascara;          // Arraste a Máscara aqui
    public float velocidadeMover = 2f;  // Velocidade de subida e descida
    public float alturaMaxima = 3f;     // Quantos metros ela vai subir
    public float tempoNoAlto = 2f;      // Quanto tempo ela fica flutuando antes de descer
    public float distanciaParaInteragir = 3f; 
    public GameObject textoInteracao;   
    
    private Transform player;
    private bool jaInteragiu = false;
    private bool podeInteragir = false;
    
    private Vector3 posicaoInicial;
    private Vector3 posicaoAlvo;

    void Start()
    {
        // Encontra o jogador automaticamente pela Tag
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;

        // Guarda a posição final da máscara
        if (mascara != null)
        {
            posicaoInicial = mascara.position;
            posicaoAlvo = mascara.position + Vector3.up * alturaMaxima;
        }
        
        // Garante que o texto comece desligado
        if (textoInteracao != null) textoInteracao.SetActive(false);
    }

    void Update()
    {
        if (player == null || jaInteragiu) return;

        float distancia = Vector3.Distance(transform.position, player.position);

        if (distancia <= distanciaParaInteragir)
        {
            if (!podeInteragir)
            {
                podeInteragir = true;
                if (textoInteracao != null) textoInteracao.SetActive(true);
            }

            if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
            {
                jaInteragiu = true;
                podeInteragir = false;
                if (textoInteracao != null) textoInteracao.SetActive(false);
                
                // Inicia a sequência de flutuar e voltar
                StartCoroutine(SequenciaFlutuar());
            }
        }
        else
        {
            if (podeInteragir)
            {
                podeInteragir = false;
                if (textoInteracao != null) textoInteracao.SetActive(false);
            }
        }
    }

    System.Collections.IEnumerator SequenciaFlutuar()
    {
        // 1. Sobe até a altura máxima
        while (Vector3.Distance(mascara.position, posicaoAlvo) > 0.01f)
        {
            mascara.position = Vector3.MoveTowards(mascara.position, posicaoAlvo, velocidadeMover * Time.deltaTime);
            yield return null;
        }

        // 2. Espera o tempo que você definiu no alto
        yield return new WaitForSeconds(tempoNoAlto);

        // 3. Desce de volta para a posição inicial
        while (Vector3.Distance(mascara.position, posicaoInicial) > 0.01f)
        {
            mascara.position = Vector3.MoveTowards(mascara.position, posicaoInicial, velocidadeMover * Time.deltaTime);
            yield return null;
        }

        // 4. Permite interagir de novo se o jogador quiser
        jaInteragiu = false; 
    }

    // Desenha uma esfera azul no editor para você ver o alcance do clique
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, distanciaParaInteragir);
    }
}
