using UnityEngine;

public class MachineUI : MonoBehaviour
{
    [SerializeField] private MachineTool _machine;
    [SerializeField] private RequiermentContent _content;

    private void OnEnable()
    {
        _machine.OnBuildZoneEnter += OnBuildZoneEnter;
        _machine.OnBuildZoneExit += OnBuildZoneExit;
    }

    private void OnDisable()
    {
        _machine.OnBuildZoneEnter -= OnBuildZoneEnter;
        _machine.OnBuildZoneExit -= OnBuildZoneExit;
    }

    private void Start()
    {
        _content.Init(_machine);
    }

    private void OnBuildZoneExit()
    {
        _content.gameObject.SetActive(false);
    }

    private void OnBuildZoneEnter()
    {
        _content.gameObject.SetActive(true);
    }
}
