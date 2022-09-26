using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] Transform wormCamera;
    WormController[] worms;
    public static int currentWorm;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        worms = GameObject.FindObjectsOfType<WormController>();
        wormCamera = Camera.main.transform;

        for(int i = 0; i < worms.Length; i++)
        {
            worms[i].wormID = i;
        }

        NextTurn();
    }
    public bool IsMyTurn(int i)
    {
        return i == currentWorm;
    }

    public void NextTurn()
    {
        StartCoroutine(GetNextWorm());
    }

    public IEnumerator GetNextWorm()
    {
        int nextWorm = currentWorm + 1;
        currentWorm -= 1;

        yield return new WaitForSecondsRealtime(2f);
        currentWorm = nextWorm;

        if(currentWorm >= worms.Length)
        {
            currentWorm = 0;
        }

        wormCamera.SetParent(worms[currentWorm].transform);
        wormCamera.localPosition = Vector3.zero + Vector3.back * 10f;
    }
    
    void Update()
    {
        
    }
}
