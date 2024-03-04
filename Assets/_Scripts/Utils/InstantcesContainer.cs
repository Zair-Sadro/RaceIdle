using UnityEngine;

public class InstantcesContainer : MonoBehaviour
{
    private static InstantcesContainer instance;
    public static InstantcesContainer Instance => instance;

    [SerializeField] private RaceIdleGame _raceIdleGame;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private TileSetter _tileSetter;
    [SerializeField] private ResourceTilesSpawn _resourceTilesSpawn;
    [SerializeField] private UIMemmory _UIMemmory;
    [SerializeField] private WalletSystem _walletSystem;
    [SerializeField] private BuildSaver _buildSaver;
    [SerializeField] private StatsValuesInformator _statsValuesInformator;
    [SerializeField] private RaceTrackManager _raceTrackManager;
    [SerializeField] private LapControll _lapControll;
    [SerializeField] private AudioService _audioService;
    [SerializeField] private PlayerSlash _playerSlasher;

    public PlayerSlash PlayerSlasher => _playerSlasher;
    public RaceIdleGame RaceIdleGame => _raceIdleGame;
    public PlayerController PlayerController => _playerController;
    public TileSetter TileSetter => _tileSetter;
    public ResourceTilesSpawn ResourceTilesSpawn => _resourceTilesSpawn;

    public UIMemmory UIMemmory => _UIMemmory;
    public WalletSystem WalletSystem => _walletSystem;
    public BuildSaver BuildSaver => _buildSaver;
    public StatsValuesInformator StatsValuesInformator => _statsValuesInformator;
    public RaceTrackManager RaceTrackManager => _raceTrackManager;
    public LapControll LapControll => _lapControll;
    public AudioService AudioService => _audioService;



    private void Awake()
    {
        

        if (instance == null) instance = this;
        else Destroy(gameObject);
    }


}
