using UnityEngine.UI;

public class FailPanel : FishMonoClass
{
    // Start is called before the first frame update    
    void Awake()
    {
        transform.Find("Restart").GetComponent<Button>().onClick.AddListener(OnRestart);
    }

    void OnRestart()
    {
        gameObject.SetActive(false);

        PuzzleManager.GetInstance.Restart();
    }

}
