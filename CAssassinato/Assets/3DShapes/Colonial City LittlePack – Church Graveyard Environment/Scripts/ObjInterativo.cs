using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class ObjInterativo : MonoBehaviour
{

    [Header("Configurações de UI")]
    [SerializeField] private GameObject painelAvisoBotao;    // Painel que diz "Aperte E para interagir"
    [SerializeField] private GameObject painelDialogoUnico;   // O texto/diálogo real deste objeto
    [SerializeField] private float tempoDoDialogo = 4f;       // Quanto tempo o diálogo fica na tela

    [Header("Ação Especial (Ex: Animação do Livro)")]
    // Isso cria um campo igual ao de um botão no Inspector para chamar a flutuação da máscara
    [SerializeField] private UnityEvent acaoEspecialAoAtivar; 

    // TRAVA GLOBAL: Compartilhada por todos os objetos. 
    // Se um diálogo estiver na tela, nenhum outro objeto aceita comandos.
    private static bool temDialogoAtivo = false; 

    private bool playerNaArea = false;
    private bool jaInteragiu = false;

    void Start()
    {
        // Garante que tudo comece desligado
        if (painelAvisoBotao != null) painelAvisoBotao.SetActive(false);
        if (painelDialogoUnico != null) painelDialogoUnico.SetActive(false);
    }

    // 1. O PLAYER ENTROU NA ÁREA
    private void OnTriggerEnter(Collider other)
    {
        // Se já interagiu com esse objeto antes, ou se já tem outro diálogo na tela, ignora
        if (jaInteragiu || temDialogoAtivo) return;

        if (other.CompareTag("Player"))
        {
            playerNaArea = true;
            if (painelAvisoBotao != null) painelAvisoBotao.SetActive(true); // Mostra "Aperte E"
        }
    }

    // O PLAYER SAIU DA ÁREA (Sem apertar o botão)
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNaArea = false;
            if (painelAvisoBotao != null) painelAvisoBotao.SetActive(false); // Esconde o aviso
        }
    }

    void Update()
    {
        // Se o player não está na área, se já interagiu, ou se o jogo está travado por outro diálogo, para aqui
        if (!playerNaArea || jaInteragiu || temDialogoAtivo) return;

        // 2. O PLAYER APERTOU O BOTÃO DE INTERAÇÃO
        if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
        {
            jaInteragiu = true;        // Tranca ESTE objeto para acontecer APENAS UMA VEZ
            temDialogoAtivo = true;    // Tranca o jogo GLOBALMENTE (nenhum outro objeto aceita clique agora)
            
            if (painelAvisoBotao != null) painelAvisoBotao.SetActive(false); // Some com o aviso de "Aperte E"

            StartCoroutine(SequenciaInteracao());
        }
    }

    IEnumerator SequenciaInteracao()
    {
        // 3. ATIVA A AÇÃO ESPECIAL (Chama a animação da máscara se for o livro)
        if (acaoEspecialAoAtivar != null) 
        {
            acaoEspecialAoAtivar.Invoke();
        }

        // 4. MOSTRA O DIÁLOGO REAL DESTE OBJETO
        if (painelDialogoUnico != null) painelDialogoUnico.SetActive(true);

        // 5. ESPERA O TEMPO QUE VOCÊ DEFINIU
        yield return new WaitForSeconds(tempoDoDialogo);

        // 6. SOME COM O DIÁLOGO
        if (painelDialogoUnico != null) painelDialogoUnico.SetActive(false);

        // 7. DESTRANCA O JOGO GLOBALMENTE (Próximos objetos já podem ser usados)
        temDialogoAtivo = false; 

        // Opcional: Destrói o colisor/gatilho para garantir que o player nunca mais ative o Trigger
        Destroy(GetComponent<Collider>());
    }
    
}
