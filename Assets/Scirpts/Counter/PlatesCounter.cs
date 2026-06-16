using UnityEngine;
using System;

// class chứa các đĩa cho player lấy
public class PlatesCounter : BaseCounter
{
    // event khi tạo ra đĩa
    public event EventHandler OnPlateSpawned;
    // event khi lấy đĩa bị player lấy mất
    public event EventHandler OnPlateRemoved;
    // kichenobject So chứa dữ liệu về đĩa như prefab để spawn, ...
    [SerializeField] private KitChenObjectSO plateKitchenObjectSO;
    // bộ đếm thời gian
    private float spawnPlateTimer;
    // thời gian sinh ra đĩa 1 đĩa = 4s
    private float spawnPlateTimerMax = 4f;
    // số đĩa đã tạo ra
    private int platesSpawnedAmount;
    // số lượng đĩa tối đa tạo ra
    private int platesSpawnedAmountMax = 4;

    // chạy update loop liên tục
    private void Update()
    {
        // cộng thêm thời gian bộ đếm thời gian lên
        spawnPlateTimer += Time.deltaTime;
        // nếu thời gian bộ đếm > thời gian max tạo ra 1 cái đĩa
        if(spawnPlateTimer > spawnPlateTimerMax)
        {
            // reset bộ đếm
            spawnPlateTimer = 0f;
            // nếu đang trong chế độ playing && số lượng nhỏ < số lượng đĩa max
            if(GameManager.Instance.IsGamePlaying() && platesSpawnedAmount < platesSpawnedAmountMax)
            {
                // tăng lên 1
                platesSpawnedAmount++;
                // Invoke gọi ra sự kiện spawn
                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }
    // gọi hàm khi tương tác phím e vs counter bàn
    public override void Interact(Player player)
    {
        // nếu player k có kitchenobject thì thực thi if
        if(!player.HasKitchenObject())
        {
            // nếu số lượng đĩa > 0 
            if(platesSpawnedAmount > 0)
            {
                // trừ đi 1 cái
                platesSpawnedAmount--;
                // spawn ra 1 cái và gắn lên player
                KitChenObject.SpawnKitchenObject(plateKitchenObjectSO, player);
                // gọi event xóa đĩa đi
                OnPlateRemoved?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
