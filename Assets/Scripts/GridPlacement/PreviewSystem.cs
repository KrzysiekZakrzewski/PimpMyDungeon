using UnityEngine;
using DG.Tweening;
using Item;

namespace GridPlacement
{
    public class PreviewSystem : MonoBehaviour
    {
        private float duration = 0.5f;
        private Color validateColor = new Color(0f, 1f, 0f, 0.5f);
        private Color rejectColor = new Color(1f, 0f, 0f, 0.5f);

        private SpriteRenderer spriteRenderer;
        private Vector3 offSet = Vector3.zero;
        private Vector2 size;
        private GridData gridData;

        private void CalculateOffSet(Vector3 size)
        {
            offSet = new Vector3(size.x / 2, size.y / 2);
        }

        private bool CheckValidate(Vector2Int position, Vector2 size)
        {
            return gridData.CheckValidation(position, size);
        }

        public void StartShow(ItemBase item)
        {
            size = item.Size;

            CalculateOffSet(size);

            spriteRenderer = item.Renderer;

            spriteRenderer.DOColor(rejectColor, duration);
        }

        public void StopShow()
        {
            spriteRenderer.DOColor(Color.white, duration);
        }

        public void MovePreview(Vector3Int position)
        {
            var newPosition = position + offSet;

            spriteRenderer.transform.position = newPosition;

            bool isValidate = CheckValidate(new Vector2Int(position.x, position.y), size);

            spriteRenderer.color = isValidate ? validateColor : rejectColor;
        }

        public void SetupGridData(GridData gridData)
        {
            this.gridData = gridData;
        }
    }
}