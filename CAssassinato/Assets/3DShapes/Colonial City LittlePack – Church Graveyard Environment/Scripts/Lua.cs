using UnityEngine;

public class Lua : MonoBehaviour
{

    private float velocidadeG = 50f;

    private Vector3 eixoR = Vector3.down;

    void Update()
    {

        transform.Rotate(eixoR * velocidadeG * Time.deltaTime);
        
    }
}
