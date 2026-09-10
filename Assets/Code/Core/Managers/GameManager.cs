using RED.Utility.Singleton;

public class GameManager : Singleton<GameManager>
{
    public GameState CurrentState { get; private set; }
    
    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        ChangeState(GameState.Menu);
    }
    
    private void OnEnable()
    {
        GameEvents.OnPlayerDied += HandlePlayerDeath;
        GameEvents.OnLevelGenerated += HandleLevelGenerated;
    }

    private void OnDisable()
    {
        GameEvents.OnPlayerDied -= HandlePlayerDeath;
        GameEvents.OnLevelGenerated -= HandleLevelGenerated;
    }
    
    public void ChangeState(GameState newState)
    {
        if (CurrentState == newState) return;
        
        CurrentState = newState;
        GameEvents.OnGameStateChanged?.Invoke(newState);

        switch (newState)
        {
            case GameState.Generating:
                GameEvents.OnRequestLevelGeneration?.Invoke();
                break;
            case GameState.Playing:
                GameEvents.OnPlayerSpawn?.Invoke();
                break;
        }
    }

    public void StartGame()
    {
        LoadingScreenManager.Instance.ShowLoadingScreen();
        
        AppSceneManager.Instance.LoadSceneAsync(AppSceneManager.Instance.GameScene, () => 
        {
            ChangeState(GameState.Generating);
        });
    }

    private void HandleLevelGenerated()
    {
        ChangeState(GameState.Playing);
        LoadingScreenManager.Instance.HideLoadingScreen();
    }

    private void HandlePlayerDeath()
    {
        ChangeState(GameState.GameOver);
    }
    
    public void TogglePause()
    {
        if (CurrentState == GameState.Playing)
        {
            ChangeState(GameState.Paused);
        }
        else if (CurrentState == GameState.Paused)
        {
            ChangeState(GameState.Playing);
        }
    }
}
