using UnityEngine;
using UnityEngine.UI;
public class TheGoblinPosition : MonoBehaviour
{
    public GameObject goblinPF;
    [SerializeField] Text deadGoblinsCount;

    int deadGoblins = 0;
    void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Arrow":
                RandomPositionForGoblin();
                deadGoblinsCount.text = Mathf.Floor(deadGoblins).ToString();
                break;
            default:
                break;
        }
    }
    void RandomPositionForGoblin()
    {
        deadGoblins += 1;
        float positionX = Random.Range(-79.8359f, 25.06841f);
        float positionY = 4.6f;
        float positionZ = Random.Range(-122.2255f, 3.474457f);

        gameObject.transform.position = new Vector3(positionX, positionY, positionZ);
    }
}
