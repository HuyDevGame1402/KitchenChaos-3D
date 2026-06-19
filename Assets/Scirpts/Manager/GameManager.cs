using UnityEngine;
using System;

// phụ trách quản lý điều khiển của game
public class GameManager : MonoBehaviour
{
    // singleton design pattern
    public static GameManager Instance { get; private set; }

    // event thay đổi state của game
    public event EventHandler OnStateChanged;
    // event pause game
    public event EventHandler OnGamePaused;
    // event hết pause game
    public event EventHandler OnGameUnpaused;

    // state của game tất cả các trạng thái
    private enum State
    {
        WaitingToStart, // trc khi bắt đầu game hay còn nói game chưa chạy timer chưa đếm chưa có gì cả giống hd đầu game ý
        CountdownToStart, // đếm thời gian trc khi bắt đầu
        GamePlaying, // đang chs game
        GameOver, // thua game
    }
    // biến để lưu state của game hiện tại
    private State state;
    // thời gian đếm ngc để bắt đầu game
    private float countdownToStartTimer = 3f;
    // bộ đếm tg game playing
    private float gamePlayingTimer;
    // tg game playing max
    private float gamePlayingTimerMax = 10f;
    // biến bool trạng thái pause hay k 
    private bool isGamePaused = false;
    
    // khởi tạo singleton với state 
    private void Awake()
    {
        Instance = this;
        state = State.WaitingToStart;
    }
    // khởi tạo bắt event game
    private void Start()
    {
        // event pause game
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
        // event tương tác với counter bàn
        GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;
    }

    // hàm thực thi khi tg tác vs counter
    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        // nếu đang ở waiting đợi ấn thì chuyển sang đếm tg để chs
        if(state == State.WaitingToStart)
        {
            state = State.CountdownToStart;
            // thay đổi trạng thái state để cập nhật UI vs sound hay music ...
            OnStateChanged?.Invoke(this, EventArgs.Empty);
        }
    }
    // hàm pause
    private void GameInput_OnPauseAction(object sender, EventArgs e)
    {
        TogglePauseGame();
    }

    private void Update()
    {
        // check các state liên tục
        switch (state)
        {
            // lúc vừa start game thì bỏ qua lúc này thg sẽ có UI hg dẫn hiển thị
            case State.WaitingToStart:
                
                break;
            // còn ở trạng thái countdown
            case State.CountdownToStart:
                // trừ dần tg countdown
                countdownToStartTimer -= Time.deltaTime;
                // nếu tg <= 0 -> hết tg countdown
                if (countdownToStartTimer <= 0f)
                {
                    // chuyển sang game playing
                    state = State.GamePlaying;
                    // đặt thời gian gameplaying vào bộ đếm tg
                    gamePlayingTimer = gamePlayingTimerMax;
                    // gọi event thay đổi state
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            
            // trạng thái game playing
            case State.GamePlaying:
                // trừ dần bộ đếm thời gian gameplaying
                gamePlayingTimer -= Time.deltaTime;
                // tg < 0 thì game over và gọi chuyển đổi state event
                if (gamePlayingTimer <= 0f)
                {
                    state = State.GameOver;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GameOver:
                break;
        }
    }
    // lấy ra trạng thái game playing
    public bool IsGamePlaying()
    {
        return state == State.GamePlaying;
    }
    // lấy trạng thái countdown
    public bool IsCountdownToStartActive()
    {
        return state == State.CountdownToStart;
    }
    // lấy ra thời gian countdown
    public float GetCountdownToStartTimer()
    {
        return countdownToStartTimer;
    }
    // check game over
    public bool IsGameOver()
    {
        return state == State.GameOver;
    }
    // lấy ra % thời gian còn lại khi chs game để chạy UI vòng tròn thời gian game chạy
    public float GetGamePlayingTimerNormalized()
    {
        return  1 - gamePlayingTimer / gamePlayingTimerMax;
    }
    // hàm thực thi khi ấn pause game
    public void TogglePauseGame()
    {
        // thay đổi trạng thái liên tục mỗi lần ấn
        isGamePaused = !isGamePaused;
        // nếu pause thì gọi event pause game 
        // và đóng băng thời gian
        if(isGamePaused)
        {
            OnGamePaused?.Invoke(this, EventArgs.Empty);
            Time.timeScale = 0f;
        }
        // ngược lại gọi hàm unpause và đặt lại tg là 1
        else
        {
            OnGameUnpaused?.Invoke(this, EventArgs.Empty);
            Time.timeScale = 1f;
        }
    }
}
