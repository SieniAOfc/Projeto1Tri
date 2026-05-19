using UnityEngine;
using System.Collections.Generic;

public class Luz : MonoBehaviour
{

    // Arraste o seu Spot e seu Point light para esta lista no Inspector
    [SerializeField] List<Light> luzesDoObjeto = new List<Light>(); 
    [SerializeField] float distanciaParaAtivar = 10f; 
    [SerializeField] float duracaoDoPisca = 2.5f; // Quanto tempo (em segundos) o susto vai durar
    [SerializeField] float tempoMinimoItem = 0.05f;
    [SerializeField] float tempoMaximoItem = 0.2f;
    [SerializeField] bool apagarNoFinal = false;

    private Transform player;
    private bool jaPiscou = false;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;
    }

    void Update()
    {
        if (player == null || jaPiscou) return;

        // Calcula a distância
        float distancia = Vector3.Distance(transform.position, player.position);

        // Se o player chegou perto, solta o pisca apenas UMA vez
        if (distancia <= distanciaParaAtivar)
        {
            StartCoroutine(SequenciaDePisca());
        }
    }

    System.Collections.IEnumerator SequenciaDePisca()
    {
        jaPiscou = true; // Bloqueia para não ativar esse código de novo em loop
        float tempoDecorrido = 0f;

        // Pisca apenas durante o tempo que você escolheu na variável 'duracaoDoPisca'
        while (tempoDecorrido < duracaoDoPisca)
        {
            float tempoEspera = Random.Range(tempoMinimoItem, tempoMaximoItem);
            tempoDecorrido += tempoEspera;

            foreach (Light luz in luzesDoObjeto)
            {
                if (luz != null) luz.enabled = !luz.enabled;
            }

            yield return new WaitForSeconds(tempoEspera);
        }

        // Acabou o tempo do susto! Define o que acontece com as luzes
        foreach (Light luz in luzesDoObjeto)
        {
            if (luz != null)
            {
                // Se 'apagarNoFinal' estiver marcado, desliga. Se não, deixa acesa normal.
                luz.enabled = !apagarNoFinal; 
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distanciaParaAtivar);
    }
}
