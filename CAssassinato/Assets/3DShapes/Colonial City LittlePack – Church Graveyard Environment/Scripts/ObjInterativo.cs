using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class ObjInterativo : MonoBehaviour
{

    [Header("Configurações de UI")]
    [SerializeField] private GameObject painelDialogoUnico;   // O texto/diálogo real deste objeto
    [SerializeField] private float tempoDoDialogo = 4f;       // Quanto tempo o diálogo fica na tela

    [Header("Ação Especial (Ex: Animação do Livro)")]
    [SerializeField] private UnityEvent acaoEspecialAoAtivar; 

    // TRAVA GLOBAL: Compartilhada por todos os objetos. 
    // Se um diálogo estiver na tela, nenhum outro objeto aceita comandos.
    private static bool temDialogoAtivo = false; 

    private bool jaInteragiu = false;

    void Start()
    {
        // Garante que o diálogo comece desligado
        if (painelDialogoUnico != null) painelDialogoUnico.SetActive(false);
    }

    // O PLAYER ENTROU NA ÁREA - A interação acontece aqui automaticamente
    private void OnTriggerEnter(Collider other)
    {
        // SE JÁ INTERAGIU: Ignora.
        // SE JÁ TEM DIÁLOGO ATIVO NO JOGO: Ignora para que um não sobrescreva o outro.
        if (jaInteragiu || temDialogoAtivo) return;

        if (other.CompareTag("Player"))
        {
            jaInteragiu = true;        // Tranca ESTE objeto para acontecer APENAS UMA VEZ
            temDialogoAtivo = true;    // Tranca o jogo GLOBALMENTE (nenhum outro objeto ativa agora)

            StartCoroutine(SequenciaInteracao());
        }
    }

    IEnumerator SequenciaInteracao()
    {
        // 1. ATIVA A AÇÃO ESPECIAL (Chama a animação da máscara se for o livro)
        if (acaoEspecialAoAtivar != null) 
        {
            acaoEspecialAoAtivar.Invoke();
        }

        // 2. MOSTRA O DIÁLOGO REAL DESTE OBJETO
        if (painelDialogoUnico != null) painelDialogoUnico.SetActive(true);

        // 3. ESPERA O TEMPO QUE VOCÊ DEFINIU
        yield return new WaitForSeconds(tempoDoDialogo);

        // 4. SOME COM O DIÁLOGO
        if (painelDialogoUnico != null) painelDialogoUnico.SetActive(false);

        // 5. DESTRANCA O JOGO GLOBALMENTE (Próximos objetos já podem ser usados)
        temDialogoAtivo = false; 

        // Destrói o colisor/gatilho para garantir que o player nunca mais ative o Trigger por segurança
        Collider colisor = GetComponent<Collider>();
        if (colisor != null)
        {
            Destroy(colisor);
        }
    }
    
}
