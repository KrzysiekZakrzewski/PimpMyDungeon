using GridPlacement;
using Levels;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

public class LevelGridHolder : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer backgroundRenderer;
    [SerializeField]
    private Grid grid;
    [SerializeField]
    private Tilemap floorTilemap, wallTilemap;
    [SerializeField]
    private Transform obstacleMagazine, itemMagazine, tipObjectsMagazine;

    [Inject]
    private void Inject(LevelManager levelManager, PlacementSystem placementSystem)
    {
        levelManager.SetupLevelBuilder(floorTilemap, wallTilemap, obstacleMagazine, itemMagazine, tipObjectsMagazine, backgroundRenderer);
        placementSystem.InjectGrid(grid);
    }
}
