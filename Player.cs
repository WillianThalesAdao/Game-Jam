using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Variaveis iniciais")] //Variaveis para encurtar o nome dos bangui
    SpriteRenderer sprite;
    Rigidbody2D body;
    Animator anim;

    [Header("Movimento Player")] //Velocidade XD
    public float velocidade;

    [Header("Interações")] //Interações ficam aqui
    //Interação de ir no arbusto e pegar um fruta
    public static bool derrubarFruta = false;

    //Aqui eu vou declarar o efeito dos itens
    public enum ItemEffect
    {
        //são nomes aleatorias pq eu não consegui imaginar nada melhor, se quiserem trocar os nomes fiquem a vontade
        shield, levelUp, special
    }

    //Porta
    public bool abrirPorta = false;

    [Header("Frutas")] //Coisas pra spawnar as frutas, dependendo do lado que tu ta a fruta cai de um lado diferente do arbusto
    public GameObject[] objetoFruta;
    public static int valorFruta; //0 = Outono, 1 = Primavera, 2 = Inverno, 3 = Verao

    [Header("Vegetais")]
    public int tomate = 0;
    public int batata = 0;
    public int cenoura = 0;

    public bool pegarBatata = false;
    public bool pegarCenoura = false;
    public bool pegarTomate = false;

    public bool temBatata = false;
    public bool temCenoura = false;
    public bool temTomate = false;

    public bool plantarBatata = false;
    public bool plantarCenoura = false;
    public bool plantarTomate = false;

    public bool estaNaCenoura = false;
    public bool estaNoTomate = false;
    public bool estaNaBatata = false;

    [Header("Objetos")]
    public GameObject mensagemInteragir;
    public GameObject portaAberta;
    public GameObject fadeIn;
    public GameObject fadeOut;

    public Text qtdTomate;
    public Text qtdCenoura;
    public Text qtdBatata;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>(); //Atribuir as variaveis iniciais
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        qtdCenoura.text = cenoura.ToString();
        qtdBatata.text = batata.ToString();
        qtdTomate.text = tomate.ToString();
    }
    private void Update()
    {
        if (GameManager.isStart)
        {
            //Sistema de combate (ainda incompleto)
            if (Input.GetKeyDown(KeyCode.Z)) //Se estiver andando de lado e apertar Z ele usa a animação atacando de lado
            {
                anim.SetTrigger("ataque_lado");
            }
        }
    }

    private void FixedUpdate()
    {

        if (GameManager.isStart)
        {
            //Movimentar para os lados
            Vector3 movimento = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"),0.0f);

            //Tocar as animações
            anim.SetFloat("Horizontal", movimento.x);
            anim.SetFloat("Vertical", movimento.y);
            anim.SetFloat("Magnitude", movimento.magnitude);

            //Movimenta a personagem
            transform.position = transform.position + movimento * Time.deltaTime;

            //Interagir porta

            if (Input.GetKeyDown(KeyCode.E) && abrirPorta)
            {
                Dormir();

                Invoke("Acordar", 1.5f);
            }

            //Interagir com horta

            if (estaNaBatata)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (temBatata)
                    {
                        pegarBatata = true;
                    }
                    else
                    {
                        plantarBatata = true;
                    }
                }
            }

            if (estaNaCenoura)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (temCenoura)
                    {
                        pegarCenoura = true;
                    }
                    else
                    {
                        plantarCenoura = true;
                    }
                }
            }

            if (estaNoTomate)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (temTomate)
                    {
                        pegarTomate = true;
                    }
                    else
                    {
                        //plantarTomate = true;
                        StartCoroutine(PlantarOTomate());
                    }
                }
            }

            if (pegarBatata)
            {
                batata += 2;
                qtdBatata.text = batata.ToString();
            }

            if (pegarCenoura)
            {
                cenoura += 3;
                qtdCenoura.text = cenoura.ToString();
            }

            if (pegarTomate)
            {
                tomate += 4;
                qtdTomate.text = tomate.ToString();
                pegarTomate = false;
            }
        }
    }

    //Desativar o fadeOut depois de alguns segundos
    IEnumerator desativarFadeOut()
    {
        yield return new WaitForSeconds(2.5f);
        fadeOut.SetActive(false);
    }

    IEnumerator PlantarOTomate()
    {
        yield return new WaitForSeconds(10f);
        temTomate = true;
    }

    IEnumerator PlantarACenoura()
    {
        yield return new WaitForSeconds(15f);
        temCenoura = true;
    }

    IEnumerator PlantarABatata()
    {
        yield return new WaitForSeconds(20f);
        temBatata = true;
    }

    void Flip() //Só flipa o personagem
    {
        sprite.flipX = !sprite.flipX;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("arbusto"))
        {
            Arbusto.podePegar = 1;
            derrubarFruta = true;
        }

        if (collision.gameObject.CompareTag("porta"))
        {
            mensagemInteragir.SetActive(true);

            abrirPorta = true;
        }

        //Colisores dos vegetais nas suas hortas
        if (collision.gameObject.CompareTag("batata"))
        {
            estaNaBatata = true;
        }

        if (collision.gameObject.CompareTag("cenoura"))
        {
            estaNaCenoura = true;
        }

        if (collision.gameObject.CompareTag("tomate"))
        {
            estaNoTomate = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Se sair de perto do arbusto a mensagem some
        if (collision.gameObject.CompareTag("porta"))
        {
            mensagemInteragir.SetActive(false);
            abrirPorta = false;
        }
    }

    void Dormir()
    {
        GameManager.isStart = false;
        portaAberta.SetActive(true);
        fadeIn.SetActive(true);
        //transform.position = new Vector2(0,);
    }

    void Acordar()
    {
        GameManager.isStart = true;
        portaAberta.SetActive(false);
        fadeIn.SetActive(false);
        fadeOut.SetActive(true);
        abrirPorta = false;
        mensagemInteragir.SetActive(false);
        StartCoroutine(desativarFadeOut());
    }

    //quando a gente encostar em algum dos itens, essa função vai ser responsavel por dizer qual o efeito
    public void SetItemEffect(ItemEffect effect)
    {
        if (effect == ItemEffect.levelUp)
        {
            int danoLevel = 0;
            danoLevel++;
            if (danoLevel >= 3)
                danoLevel = 3;
        }
    }
}
