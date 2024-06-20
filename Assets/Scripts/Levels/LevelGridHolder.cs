using GridPlacement;
using Levels;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

public class LevelGridHolder : MonoBehaviour
{
    [SerializeField]
    private Grid grid;
    [SerializeField]
    private Tilemap floorTilemap, wallTilemap;
    [SerializeField]
    private Transform obstacleMagazine, itemMagazine;

    [Inject]
    private void Inject(LevelManager levelManager, PlacementSystem placementSystem)
    {
        levelManager.SetupLevelBuilder(floorTilemap, wallTilemap, obstacleMagazine, itemMagazine);
        placementSystem.InjectGrid(grid);
    }
}
