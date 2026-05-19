using System.Collections;
using UnityEngine;

public class DLuz : MonoBehaviour
{
    [SerializeField] private GameObject caixaDeDialogo; // Arraste seu painel de UI aqui
    [SerializeField] private float tempoParaSumir = 4.0f; // Tempo em segundos

    private bool jaAtivou = false; // Garante que só aconteça uma vez

    private void OnTriggerEnter(Collider other)
    {
        // Verifica se quem entrou na área foi o Player e se ainda não foi ativado
        if (other.CompareTag("Player") && !jaAtivou)
        {
            jaAtivou = true;
            StartCoroutine(MostrarEDestruirDialogo());
        }
    }

    // Uma Corrotina serve perfeitamente para contar tempo sem travar o jogo
    IEnumerator MostrarEDestruirDialogo()
    {
        caixaDeDialogo.SetActive(true); // Mostra a caixa

        yield return new WaitForSeconds(tempoParaSumir); // Espera o tempo definido

        caixaDeDialogo.SetActive(false); // Some com a caixa
        
        // Opcional: Destrói este gatilho para liberar memória, já que não vamos usar mais
        Destroy(gameObject); 
    }
}

