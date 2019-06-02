public class GameSingleton : Singleton<GameSingleton>
{
    // (Optional) Prevent non-singleton constructor use.
    protected GameSingleton() { }

    // Then add whatever code to the class you need as you normally would.
    public string MyTestString = "Hello world!";

    public int GameLevel = 0;
    public int SavedCargo = 0;
    public int LostCargo = 0;
    public int TotalSavedCargo = 0;
    public int TotalLostCargo = 0;

    public int HaulerCount = 3;
    public int LostHaulers = 0;
    public int LostHaulersTotal = 0;

    public int maxEmy = 6;
    public int emyPerSpawn = 6;
    public float spawnInterval = 45f;
    public float spawnBossInterval = 120f;
    public int bossPerSpawn = 1;
    public float lieutenantOdds = 20.0f;

    public float levelStartTime = 120f;

    public void DecrementHauler()
    {
        HaulerCount = HaulerCount <= 0 ? 0 : HaulerCount - 1;
    }
}
