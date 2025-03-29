using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class AxeControl : MonoBehaviour
{
    Rigidbody rigidBody;
    AudioSource audioSource;
    AudioSource flySound;
    enum State { Play, Paused, Dead, Finish };
    enum Regime { Infinity, Ordinary };
    [SerializeField] float moveSpeed = 0f;
    [SerializeField] float upSpeed = 0f;
    [SerializeField] float rotationSpeed = 0f;
    [SerializeField] float totalPower = 400;
    [SerializeField] Text title;
    [SerializeField] Text power;
    [SerializeField] Text pause;
    [SerializeField] Text lose;
    [SerializeField] ParticleSystem flyParticle;
    [SerializeField] ParticleSystem particleSystemC;
    [SerializeField] ParticleSystem particleSystemA;
    [SerializeField] GameObject BloodParticle;
    [SerializeField] AudioClip Hit;
    [SerializeField] AudioClip Lose;
    [SerializeField] AudioClip Fanfare;
    [SerializeField] Regime regime;
    State state;
    void Start()
    {
        state = State.Play;
        flySound = GetComponentInChildren<AudioSource>();
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        // получаем доступ к партиклям и при наличии достаем их из обьектов
        Transform partA = gameObject.transform.Find("BloodSpray/Particle System_A");
        Transform partC = gameObject.transform.Find("BloodSpray/Particle System_C");
        if (partA != null && partC != null)
        {
            particleSystemA = partA.GetComponent<ParticleSystem>();
            particleSystemC = partC.GetComponent<ParticleSystem>();
        }
    }
    void Update()
    {
        PausedGame();
        if (state == State.Paused)
        {
            PauseMenu();
        }
        if (state == State.Dead || totalPower <= 0)
        {
            Dead();
        }
        if (state == State.Play)
        {
            SpeedControl();
            Rotation();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Finish":
                Finish();
                break;
            case "Friendly":
                break;
            default:
                Dead();
                break;
        }
    }
    void PausedGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (state != State.Paused)
            {
                pause.text = "PAUSED";
                Time.timeScale = 0;
                state = State.Paused;
                Debug.Log("PAUSED | TimeScale: " + Time.timeScale);
            }
            else
            {
                pause.text = "";
                Time.timeScale = 1;
                state = State.Play;
                Debug.Log("UNPAUSED | TimeScale: " + Time.timeScale);
            }
        }
    }
    void PauseMenu()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene(0);
        }
    }
    void SpeedControl()
    {
        Vector3 moveDirection = transform.forward;
        if (Input.GetKey(KeyCode.Space))
        {
            Time.timeScale = 2;
            totalPower -= Mathf.RoundToInt(3 * Time.deltaTime);
            rigidBody.AddForce(-moveDirection * upSpeed * Time.deltaTime);
        }
        else
        {
            Time.timeScale = 1;
            rigidBody.AddForce(-moveDirection * moveSpeed * Time.deltaTime);
        }
    }
    void Rotation()
    {
        if (regime == Regime.Infinity)
        {
            title.text = "Dead Goblins ";
        }
        else
        {
            totalPower -= 1f * Time.deltaTime;
            title.text = "Energy ";
            power.text = Mathf.RoundToInt(totalPower).ToString();
        }
        rigidBody.freezeRotation = true;

        if (Input.GetKey(KeyCode.W))
        {
            transform.Rotate(-Vector3.left * rotationSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Rotate(Vector3.left * rotationSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(-Vector3.up * rotationSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        }
    }
    void Finish()
    {
        particleSystemA.Play();
        particleSystemC.Play();
        flySound.Stop();
        flyParticle.Stop();
        audioSource.Stop();
        audioSource.PlayOneShot(Hit);
        audioSource.PlayOneShot(Fanfare);
        state = State.Finish;
        Invoke("NextLevel", 2f);
    }
    void Dead()
    {
        rigidBody.freezeRotation = false;
        audioSource.Stop();
        flySound.Stop();
        flyParticle.Stop();
        audioSource.PlayOneShot(Lose);
        lose.text = "You Lose";
        state = State.Dead;
        Invoke("FirstLevel", 1.5f);
    }
    void FirstLevel()
    {
        SceneManager.LoadScene(1);
    }
    void NextLevel()
    {
        int nextLevelIndex = SceneManager.GetActiveScene().buildIndex + 1;

        // last scene infinite regime
        if (nextLevelIndex == SceneManager.sceneCountInBuildSettings - 1)
        {
            nextLevelIndex = 1;
        }
        SceneManager.LoadScene(nextLevelIndex);
    }
}
