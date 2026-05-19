using UnityEngine;
using System.Collections.Generic;

public class livro : MonoBehaviour
{
    [SerializeField] Transform mascara;          // Arraste a Máscara aqui
    [SerializeField] float velocidadeMover = 2f;  // Velocidade de subida e descida
    [SerializeField] float alturaMaxima = 3f;     // Quantos metros ela vai subir
    [SerializeField] float tempoNoAlto = 1f;      // Quanto tempo ela fica flutuando antes de descer

    
    private Vector3 posicaoInicial;
    private Vector3 posicaoAlvo;

    void Start()
    {
        // Guarda as posições iniciais e finais baseadas na posição atual do objeto
        if (mascara != null)
        {
            posicaoInicial = mascara.position;
            posicaoAlvo = mascara.position + Vector3.up * alturaMaxima;
        }
    }

    // FUNÇÃO PÚBLICA: É essa função que o outro script vai chamar para dar o "play"
    public void AtivarFlutuarMascara()
    {
        StartCoroutine(SequenciaFlutuar());
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
    }
}
