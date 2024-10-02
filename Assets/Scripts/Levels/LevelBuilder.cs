using Audio.Manager;
using DG.Tweening;
using Extensions;
using Generator.Data;
using GridPlacement;
using Item;
using Item.Guide;
using Levels.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

namespace Generator
{
    public class LevelBuilder : MonoBehaviour
    {
        [SerializeField]
        private TileMapVisualizerDatabase database;

        private LevelDataSO levelDataSO;

        private readonly float extraBound = 0.5f;

        private readonly float waitDuration = 0.1f;

        private Vector3 screenSizeOffset = new(3f, 3f);
        private Vector2 screenSizeAdder = new(0.05f, 0.05f);
        private float percentOffsetMultiply = 0.95f;

        private HashSet<Vector2Int> floorInHash;
        private HashSet<Vector2Int> obstaclesInHash;
        private List<Bounds> objectsBounds;
        private Camera currentCamera;
        private GridData gridData;
        private PlacementSystem placementSystem;
        private ItemGuideController itemGuide;
        private AudioManager audioManager;
        private Tilemap floorTilemap, wallTilemap;
        private Transform obstacleMagazine, itemMagazine, tipObjectsMagazine;
        private SpriteRenderer backgroundRenderer;
        private List<PlaceableItem> placeableItems;

        private bool squareTipIsPlaying;

        private RectTransform[] rectsToIgnore;
        private Bounds[] boundsToIgnore;

        public Transform TipObjectsMagazine => tipObjectsMagazine;

        [Inject]
        private void Inject(PlacementSystem placementSystem, AudioManager audioManager, ItemGuideController itemGuide)
        {
            this.placementSystem = placementSystem;
            this.audioManager = audioManager;
            this.itemGuide = itemGuide;
        }

        private void ClearObjectives()
        {
            objectsBounds = new List<Bounds>();

            objectsBounds.Clear();
        }

        private void OnOffSquareVisualizer(bool onOff)
        {
            foreach (PlaceableItem item in placeableItems)
            {
                item.OnOffSquareVisualize(onOff);
            }
        }

        private IEnumerator ShowTipSquareSequance()
        {
            squareTipIsPlaying = true;

            OnOffSquareVisualizer(true);

            yield return new WaitForSeconds(2f);

            OnOffSquareVisualizer(false);

            yield return new WaitForSeconds(1f);

            squareTipIsPlaying = false;
        }

        private void SetCameraPosition()
        {
            Vector3 cameraPosition =
                new(levelDataSO.CenterPoint.x,
                levelDataSO.CenterPoint.y,
                currentCamera.transform.position.z);

            currentCamera.transform.position = cameraPosition;

            backgroundRenderer.transform.position = new Vector3(cameraPosition.x, cameraPosition.y, 0);
        }

        private void PaintSingleBasicWall(Vector2Int position, string binaryType)
        {
            int typeAsInt = Convert.ToInt32(binaryType, 2);
            TileBase tile = null;
            if (WallTypesHelper.wallTop.Contains(typeAsInt))
            {
                tile = database.WallTop;
            }
            else if (WallTypesHelper.wallSideRight.Contains(typeAsInt))
            {
                tile = database.WallSideRight;
            }
            else if (WallTypesHelper.wallSideLeft.Contains(typeAsInt))
            {
                tile = database.WallSiderLeft;
            }
            else if (WallTypesHelper.wallBottm.Contains(typeAsInt))
            {
                tile = database.WallBottom;
            }
            else if (WallTypesHelper.wallFull.Contains(typeAsInt))
            {
                tile = database.WallFull;
            }

            if (tile != null)
                PaintSingleTile(wallTilemap, tile, position);
        }

        private void PaintSingleCornerWall(Vector2Int position, string binaryType)
        {
            int typeASInt = Convert.ToInt32(binaryType, 2);
            TileBase tile = null;

            if (WallTypesHelper.wallInnerCornerDownLeft.Contains(typeASInt))
            {
                tile = database.WallInnerCornerDownLeft;
            }
            else if (WallTypesHelper.wallInnerCornerDownRight.Contains(typeASInt))
            {
                tile = database.WallInnerCornerDownRight;
            }
            else if (WallTypesHelper.wallDiagonalCornerDownLeft.Contains(typeASInt))
            {
                tile = database.WallDiagonalCornerDownLeft;
            }
            else if (WallTypesHelper.wallDiagonalCornerDownRight.Contains(typeASInt))
            {
                tile = database.WallDiagonalCornerDownRight;
            }
            else if (WallTypesHelper.wallDiagonalCornerUpRight.Contains(typeASInt))
            {
                tile = database.WallDiagonalCornerUpRight;
            }
            else if (WallTypesHelper.wallDiagonalCornerUpLeft.Contains(typeASInt))
            {
                tile = database.WallDiagonalCornerUpLeft;
            }
            else if (WallTypesHelper.wallFullEightDirections.Contains(typeASInt))
            {
                tile = database.WallFull;
            }
            else if (WallTypesHelper.wallBottmEightDirections.Contains(typeASInt))
            {
                tile = database.WallBottom;
            }

            if (tile != null)
                PaintSingleTile(wallTilemap, tile, position);
        }

        private void PaintSingleTile(Tilemap tilemap, TileBase tile, Vector2Int position)
        {
            var tilePosition = tilemap.WorldToCell((Vector3Int)position);
            tilemap.SetTile(tilePosition, tile);
        }

        private bool IsOccupiedPosition(Vector2 position, Vector3 size)
        {
            foreach(Bounds bound in objectsBounds)
            {
                Bounds bounds = new Bounds(bound.center, bound.size + size);

                if (bounds.Contains(position))
                    return true;
            }

            return false;
        }

        private bool IsOverUI(Bounds[] bounds, Vector2 position)
        {
            for (int i = 0; i < bounds.Length; i++)
            {
                if (bounds[i].Contains(position))
                    return true;
            }

            return false;
        }

        private Vector2 CalculatePositionForItem(Vector2Int size)
        {
            bool succesfull = false;

            Vector3 offSetSize = new((float)size.x * percentOffsetMultiply, (float)size.y * percentOffsetMultiply);

            Bounds objectsArea = CalculateObjectsArea(offSetSize);
            Bounds levelBounds = new(wallTilemap.cellBounds.center, wallTilemap.cellBounds.size + offSetSize);

            Vector2 position = Vector2.zero;

            Bounds[] uiIgnore = new Bounds[boundsToIgnore.Length];

            for(int i = 0; i < boundsToIgnore.Length; i++)
            {
                uiIgnore[i] = new Bounds(boundsToIgnore[i].center, boundsToIgnore[i].size + offSetSize);
            }

            while (!succesfull)
            {
                position = RandomExtension.RandomPointInBounds(objectsArea);

                bool inLevelBounds = levelBounds.Contains(position);

                bool isOverUI = IsOverUI(uiIgnore, position);

                succesfull = !inLevelBounds && !isOverUI && !IsOccupiedPosition(position, offSetSize);
            }

            Bounds objectBound = new(position, new Vector3(size.x + extraBound, size.y + extraBound, 0));

            objectsBounds.Add(objectBound);

            return position;
        }

        private void ResizeCamera()
        {
            Vector3 size = wallTilemap.cellBounds.size + screenSizeOffset;

            float screenRatio = (float)Screen.width / (float)Screen.height;
            float targetRatio = size.x / size.y;

            if (screenRatio >= targetRatio)
                currentCamera.orthographicSize = size.y / 2;
            else
            {
                float differenceInSize = targetRatio / screenRatio;
                currentCamera.orthographicSize = size.y / 2 * differenceInSize;
            }

            ResizeBackground(currentCamera.orthographicSize);
        }

        private void ResizeCamera(float orthographicSize)
        {
            ResizeBackground(orthographicSize);

            currentCamera.DOOrthoSize(orthographicSize, 1f);
        }

        private void ResizeBackground()
        {
            backgroundRenderer.transform.DOScale(CalculateBackgroundSize(), 0.5f);
        }

        private void ResizeBackground(float orthographicSize)
        {
            backgroundRenderer.transform.localScale = CalculateBackgroundSize(orthographicSize);
        }

        private Bounds CalculateObjectsArea(Vector3 offSetSize)
        {
            float worldScreenHeight = currentCamera.orthographicSize * 2f;
            float worldScreenWidth = worldScreenHeight / Screen.safeArea.height * Screen.safeArea.width;

            Vector3 size = new Vector3(worldScreenWidth, worldScreenHeight) - (offSetSize * 1.5f);

            return new Bounds(backgroundRenderer.bounds.center, size);
        }

        private void CalculateRectIgnoreBounds()
        {
            if(rectsToIgnore == null)
                rectsToIgnore = new RectTransform[0];

            boundsToIgnore = new Bounds[rectsToIgnore.Length];

            for (int i = 0; i < rectsToIgnore.Length; i++)
            {
                boundsToIgnore[i] = CalculateSingleRectIgnoreBound(rectsToIgnore[i]);
            }
        }

        private Bounds CalculateSingleRectIgnoreBound(RectTransform rect)
        {
            Vector3 rightUp = currentCamera.ScreenToWorldPoint(rect.position + new Vector3(rect.sizeDelta.x / 2, rect.sizeDelta.y / 2));
            Vector3 leftDown = currentCamera.ScreenToWorldPoint(rect.position - new Vector3(rect.sizeDelta.x / 2, rect.sizeDelta.y / 2));

            float h = Mathf.Abs(rightUp.y) - Mathf.Abs(leftDown.y);
            float w = Mathf.Abs(rightUp.x) - Mathf.Abs(leftDown.x);

            Vector3 size = new(Mathf.Abs(h), Mathf.Abs(w), wallTilemap.cellBounds.size.z);

            Vector3 rectPointInWorld = currentCamera.ScreenToWorldPoint(rect.position);

            Vector3 center = new(rectPointInWorld.x, rectPointInWorld.y, wallTilemap.cellBounds.center.z);

            return new Bounds(center, size);
        }

        private Vector3 CalculateBackgroundSize()
        {
            if (backgroundRenderer == null) 
                return Vector2.one;

            float worldScreenHeight = currentCamera.orthographicSize * 2f;
            float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

            var spriteSize = backgroundRenderer.sprite.bounds.size;
            Vector2 scale = Vector2.one;
            scale.x = worldScreenWidth / spriteSize.x;
            scale.y = worldScreenHeight / spriteSize.y;

            return scale + screenSizeAdder;
        }

        private Vector3 CalculateBackgroundSize(float orthographicSize)
        {
            if (backgroundRenderer == null)
                return Vector2.one;

            float worldScreenHeight = orthographicSize * 2f;
            float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

            var spriteSize = backgroundRenderer.sprite.bounds.size;
            Vector2 scale = Vector2.one;
            scale.x = worldScreenWidth / spriteSize.x;
            scale.y = worldScreenHeight / spriteSize.y;

            return scale + screenSizeAdder;
        }

        private bool CheckReferences()
        {
            return floorTilemap != null && wallTilemap != null;
        }

        #region Sequences

        private IEnumerator BuildlevelSequence(Action finishedEvent)
        {
            floorInHash = new HashSet<Vector2Int>(levelDataSO.FloorPositions);

            StartCoroutine(GetCameraSequnce());

            yield return new WaitUntil(() => currentCamera != null);

            SetCameraPosition();

            yield return new WaitUntil(CheckReferences);

            Clear();

            PaintWalls();
            PaintFloor();

            ResizeCamera();

            CalculateRectIgnoreBounds();

            yield return StartCoroutine(SpawnObstacles());
            yield return StartCoroutine(SpawnPlaceableItem());

            gridData = new GridData(floorInHash, obstaclesInHash);
            placementSystem.SetupGridData(gridData);

            finishedEvent?.Invoke();
        }

        private void PaintFloor()
        {
            PaintFloorTiles(floorInHash, floorTilemap, database.FloorTile);
        }

        private void PaintWalls()
        {
            var basicWallPositions = WallGenerator.FindWallsInDirections(floorInHash, Direction2D.cardinalDirectionsList);
            var cornerWallPositions = WallGenerator.FindWallsInDirections(floorInHash, Direction2D.diagonalDirectionsList);

            CreateBasicWall(basicWallPositions);

            CreateCornerWalls(cornerWallPositions);
        }

        private void CreateCornerWalls(HashSet<Vector2Int> cornerWallPositions)
        {
            foreach (var position in cornerWallPositions)
            {
                string neighboursBinaryType = "";
                foreach (var direction in Direction2D.eightDirectionsList)
                {
                    var neighbourPosition = position + direction;
                    if (floorInHash.Contains(neighbourPosition))
                    {
                        neighboursBinaryType += "1";
                    }
                    else
                    {
                        neighboursBinaryType += "0";
                    }
                }
                PaintSingleCornerWall(position, neighboursBinaryType);
            }
        }

        private void CreateBasicWall(HashSet<Vector2Int> basicWallPositions)
        {
            foreach (var position in basicWallPositions)
            {
                string neighboursBinaryType = "";
                foreach (var direction in Direction2D.cardinalDirectionsList)
                {
                    var neighbourPosition = position + direction;
                    if (floorInHash.Contains(neighbourPosition))
                    {
                        neighboursBinaryType += "1";
                    }
                    else
                    {
                        neighboursBinaryType += "0";
                    }
                }
                PaintSingleBasicWall(position, neighboursBinaryType);
            }
        }

        private void PaintFloorTiles(IEnumerable<Vector2Int> positions, Tilemap tilemap, TileBase tile)
        {
            foreach (var position in positions)
            {
                PaintSingleTile(tilemap, tile, position);
            }
        }

        private IEnumerator SpawnObstacles()
        {
            obstaclesInHash = new HashSet<Vector2Int>();

            var obstacles = levelDataSO.Obstacles;

            if (levelDataSO.Obstacles.Count == 0)
                yield break;

            var arrayOfAllKeys = obstacles.Keys.ToArray();
            var arrayOfAllValues = obstacles.Values.ToArray();

            for(int i = 0; i < obstacles.Count; i++)
            {
                var obstacleData = arrayOfAllValues[i];

                var size = obstacleData.CalculateSize();

                var offSet = new Vector3((float)size.x / 2, (float)size.y / 2);

                Vector3 spawnPosition = new Vector3(arrayOfAllKeys[i].x, arrayOfAllKeys[i].y) + offSet;

                Instantiate(obstacleData.Prefab, spawnPosition, Quaternion.identity, obstacleMagazine);

                for (int j = 0; j < obstacleData.ItemPoints.Count; j++)
                    obstaclesInHash.Add(arrayOfAllKeys[i] + obstacleData.ItemPoints[j]);

                yield return new WaitForSeconds(waitDuration);
            }
        }

        private IEnumerator SpawnPlaceableItem()
        {
            placeableItems = new();

            for (int i = 0; i < levelDataSO.PlaceableItems.Count; i++)
            {
                var item = levelDataSO.PlaceableItems[i];

                var size = item.PlaceItemData.CalculateSize();

                var position = levelDataSO.IsOverride
                    ? levelDataSO.PlaceableItemOverride[i].OverrideSpawnPosition
                    : CalculatePositionForItem(size);

                var newObject = Instantiate(item.PlaceItemData.Prefab, position, Quaternion.identity, itemMagazine);

                var newItem = newObject.GetComponent<PlaceableItem>();

                newItem.Setup(item.PlaceItemData, placementSystem, audioManager, position);

                placeableItems.Add(newItem);

                yield return new WaitForSeconds(waitDuration);

                if (levelDataSO.IsOverride)
                    continue;

                itemGuide.TryUnlockItem(item.PlaceItemData.Id);
            }
        }

        private IEnumerator GetCameraSequnce()
        {
            while(currentCamera == null)
            {
                currentCamera = Camera.main;

                yield return null;
            }
        }

        #endregion

        public void ToMainMenuClear()
        {
            StopAllCoroutines();

            Clear();
        }

        public void Clear()
        {
            floorTilemap.ClearAllTiles();
            wallTilemap.ClearAllTiles();

            for (int i = obstacleMagazine.childCount - 1; i >= 0; i--)
            {
                var obstacle = obstacleMagazine.GetChild(i);

                Destroy(obstacle.gameObject);
            }

            for (int i = itemMagazine.childCount - 1; i >= 0; i--)
            {
                var item = itemMagazine.GetChild(i);

                Destroy(item.gameObject);
            }
        }

        public void BuildLevel(LevelDataSO levelDataSO, Action finishedEvent = null)
        {
            this.levelDataSO = levelDataSO;

            ClearObjectives();

            StartCoroutine(BuildlevelSequence(finishedEvent));
        }

        public void SetupBuildReferences(Tilemap floorTilemap, Tilemap wallTilemap, Transform obstacleMagazine, Transform itemMagazine, Transform tipObjectsMagazine, SpriteRenderer backgroundRenderer)
        {
            this.floorTilemap = floorTilemap;
            this.wallTilemap = wallTilemap;
            this.obstacleMagazine = obstacleMagazine;
            this.itemMagazine = itemMagazine;
            this.backgroundRenderer = backgroundRenderer;
            this.tipObjectsMagazine = tipObjectsMagazine;
        }

        public PlaceableItem GetItemByData(ItemData itemData)
        {
            return placeableItems.Find(x => x.IsSame(itemData));
        }

        public void ChangeItemInteractable(ItemData itemData, bool isInteractable)
        {
            var item = GetItemByData(itemData);

            item.ChangeInteractableState(isInteractable);
        }

        public void OffAllItemInteractable()
        {
            for (int i = 0; i < placeableItems.Count; i++)
            {
                placeableItems[i].ChangeInteractableState(false);
            }
        }

        public void SetRects(RectTransform[] rectsToIgnore)
        {
            this.rectsToIgnore = rectsToIgnore;
        }

        public void ShowTipSquare()
        {
            if (squareTipIsPlaying)
                return;

            StartCoroutine(ShowTipSquareSequance());
        }

        public List<ItemGeneratedData> LevelItems => levelDataSO.PlaceableItems;
    } 
}
