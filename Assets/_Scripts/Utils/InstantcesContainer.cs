using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantcesContainer :MonoBehaviour
{
    private static InstantcesContainer instance;
    public static InstantcesContainer Instance => instance;

    [SerializeField] private RaceIdleGame _raceIdleGame;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private TileSetter _tileSetter;
    [SerializeField] private ResourceTilesSpawn _resourceTilesSpawn;
    [SerializeField] private InputManager _inputManager;
    [SerializeField] private UIMemmory _UIMemmory;
    [SerializeField] private WalletSystem _walletSystem;
    [SerializeField] private BuildSaver _buildSaver;
    [SerializeField] private StatsValuesInformator _statsValuesInformator;
    [SerializeField] private RaceTrackManager _raceTrackManager;

    public RaceIdleGame RaceIdleGame => _raceIdleGame;
    public PlayerController PlayerController => _playerController;
    public TileSetter TileSetter => _tileSetter;
    public ResourceTilesSpawn ResourceTilesSpawn => _resourceTilesSpawn;
    public InputManager InputManager => _inputManager;
    public UIMemmory UIMemmory => _UIMemmory;
    public WalletSystem WalletSystem => _walletSystem;
    public BuildSaver BuildSaver => _buildSaver;
    public StatsValuesInformator StatsValuesInformator => _statsValuesInformator;
    public RaceTrackManager RaceTrackManager => _raceTrackManager;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }


}
