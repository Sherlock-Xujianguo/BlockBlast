using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FailPanel : MonoBehaviour
{
    PuzzleManager PuzzleManagerInstance = PuzzleManager.GetInstance;
    // Start is called before the first frame update    
    void Start()
    {
        transform.Find("Restart").GetComponent<Button>().onClick.AddListener(() =>
        {
            PuzzleManagerInstance.Restart();
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
