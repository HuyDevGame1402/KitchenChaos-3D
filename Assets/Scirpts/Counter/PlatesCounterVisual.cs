using UnityEngine;
using System.Collections.Generic;

// class phụ trách việc spawn các đĩa trên bàn counter
public class PlatesCounterVisual : MonoBehaviour
{
    // counter chứa đĩa
    [SerializeField] private PlatesCounter platesCounter;
    // điểm spawn plate
    [SerializeField] private Transform counterTopPoint;
    // plate đĩa cần spawn ra
    [SerializeField] private Transform plateVisualPrefab;
    // ds các đĩa đã đc tạo ra
    private List<GameObject> plateVisualGameObjectList = new List<GameObject>();
    // offset y của từng đĩa cộng vào để xếp trồng lên nhau
    private float plateOffsetY = .1f;

    // start đăng ký event
    private void Start()
    {
        // event spawn đĩa
        platesCounter.OnPlateSpawned += PlatesCounter_OnPlateSpawned;
        // event remove đĩa
        platesCounter.OnPlateRemoved += PlatesCounter_OnPlateRemoved;
    }

    private void PlatesCounter_OnPlateRemoved(object sender, System.EventArgs e)
    {
        // lấy ra cái cuối cùng ở ds
        GameObject plateGameObject = plateVisualGameObjectList[
            plateVisualGameObjectList.Count - 1];
        // xóa đi trong ds list
        plateVisualGameObjectList.Remove(plateGameObject);
        // xóa cái đĩa đó đi trong game
        Destroy(plateGameObject);
    }
    // spawn plate ra
    private void PlatesCounter_OnPlateSpawned(object sender, System.EventArgs e)
    {
        // tạo ra đĩa
        Transform plateVisualTransform = Instantiate(plateVisualPrefab, counterTopPoint);
        // chỉnh lại position y để xếp chồng lên nhau
        plateVisualTransform.localPosition = new Vector3 (0, plateOffsetY * 
            plateVisualGameObjectList.Count, 0);
        // add vào ds đĩa
        plateVisualGameObjectList.Add(plateVisualTransform.gameObject);
    }
}
