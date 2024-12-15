using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCountVisual : MonoBehaviour
{
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private Transform plateVisualPrefab;
    [SerializeField] private PlatesCounter platesCounter;


    private List<GameObject> plateVisualGameObjectList;
    private Stack<GameObject> plateVisualStack;

    private void Awake()
    {
        plateVisualGameObjectList = new List<GameObject>();
        plateVisualStack = new Stack<GameObject>();
    }

    private void Start()
    {
        platesCounter.OnPlateSpawned += PlatesCounter_OnPlateSpawned;
        platesCounter.OnPlateRemoved += PlatesCounter_OnPlateRemoved;
    }

    private void PlatesCounter_OnPlateRemoved(object sender, System.EventArgs e)
    {
        //GameObject plateGameObject = plateVisualGameObjectList[plateVisualGameObjectList.Count - 1];
        //plateVisualGameObjectList.Remove(plateGameObject);
        //Destroy(plateGameObject);
        GameObject plateGameObject = plateVisualStack.Pop();
        Destroy(plateGameObject);
    }

    private void PlatesCounter_OnPlateSpawned(object sender, System.EventArgs e)
    {
       Transform plateVisualTransform = Instantiate(plateVisualPrefab, counterTopPoint);

        float plateOffsetY = 0.1f;
        //plateVisualTransform.localPosition = new Vector3 (0, plateOffsetY * plateVisualGameObjectList.Count, 0);
        plateVisualTransform.localPosition = new Vector3(0, plateOffsetY * plateVisualStack.Count, 0);

        //plateVisualGameObjectList.Add(plateVisualTransform.gameObject);
        plateVisualStack.Push(plateVisualTransform.gameObject);

    }
}
