using UnityEngine;

// quản lý sound âm thanh của game
public class SoundManager : MonoBehaviour
{
    // biến string để lưu key của volum
    private const string PLAYER_PREFS_SOUND_EFFECTS_VOLUME = "SoundEffectsVolume";

    public static SoundManager Instance { get; private set; }
    // 1 scriptable object lưu các mảng audico clip
    [SerializeField] private AudioClipRefsSO audioClipRefsSO;
    private float volume = 1f; // mặc định volume = 1f
    private void Awake()
    {
        Instance = this;
        // lấy ra volume mỗi lần mở game chạy nếu trc đã chỉnh là 0.3 thì lần sau cũng sẽ lấy đc 0.3f
        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, 1f);
    }
    // đăng ký các event để gọi sound
    private void Start()
    {
        // đăng ký nhận hàm khi gọi giao đồ ăn xong
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        // đk khi đồ ăn fail
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;
        // cắt đồ ăn
        CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;
        // cầm đồ vật
        Player.Instance.OnPickedSomething += Player_OnPickedSomething;
        // đặt đồ
        BaseCounter.OnAnyObjectPlacedHere += BaseCounter_OnAnyObjectPlacedHere;
        // bỏ đồ vào thùng rác
        TrashCounter.OnAnyObjectTrashed += TrashCounter_OnAnyObjectTrashed;
    }
    // gọi sound tại vị trí counter và phát tương ứng sound đó random trong mảng array sound
    private void TrashCounter_OnAnyObjectTrashed(object sender, System.EventArgs e)
    {
        TrashCounter trashCounter = sender as TrashCounter;
        PlaySound(audioClipRefsSO.trash, trashCounter.transform.position);
    }

    private void BaseCounter_OnAnyObjectPlacedHere(object sender, System.EventArgs e)
    {
        BaseCounter baseCounter = sender as BaseCounter;
        PlaySound(audioClipRefsSO.objectDrop, baseCounter.transform.position);
    }

    private void Player_OnPickedSomething(object sender, System.EventArgs e)
    {
        PlaySound(audioClipRefsSO.objectPickup, Player.Instance.transform.position);
    }

    private void CuttingCounter_OnAnyCut(object sender, System.EventArgs e)
    {
        CuttingCounter cuttingCounter = sender as CuttingCounter;
        PlaySound(audioClipRefsSO.chop, cuttingCounter.transform.position);
    }

    private void DeliveryManager_OnRecipeFailed(object sender, System.EventArgs e)
    {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audioClipRefsSO.deliveryFail, deliveryCounter.
            transform.position);
    }

    private void DeliveryManager_OnRecipeSuccess(object sender, System.EventArgs e)
    {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audioClipRefsSO.deliverySucess, deliveryCounter.
            transform.position);
    }

    private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volume = 1f)
    {
        PlaySound(audioClipArray[Random.Range(0, audioClipArray.Length)], position,
            volume);
    }
    private void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultiplier = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volumeMultiplier * volume);
    }
    public void PlayFootstepsSound(Vector3 position, float volume)
    {
        PlaySound(audioClipRefsSO.footstep, position, volume);
    }
    public void PlayCountdownSound()
    {
        PlaySound(audioClipRefsSO.warning, Vector3.zero, volume);
    }
    public void PlayWarningSound(Vector3 position)
    {
        PlaySound(audioClipRefsSO.warning, position, volume);
    }
    // thay đổi volume
    public void ChangeVolume()
    {
        // cộng lên 0.1f
        volume += 0.1f;
        // vượt qua ngưỡng max thì đặt lại về 0
        if(volume > 1f)
        {
            volume = 0f;
        }
        // set và lưu vào trong PlayerPrefs
        PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, volume);
        PlayerPrefs.Save();
    }
    public float GetVolume()
    {
        return volume;
    }
}
