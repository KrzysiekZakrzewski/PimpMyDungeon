using Audio.SoundsData;
using UnityEngine;
using Haptics;
using UnityEngine.UI;
using Audio.Manager;
using Zenject;
using UnityEngine.Events;

public class UIButton : MonoBehaviour
{
    [SerializeField]
    protected Button button;
    [SerializeField]
    private SoundSO pressSound;
    [SerializeField]
    private HapticsType hapticsType;

    private AudioManager audioManager;

    private RectTransform rect;

    [Inject]
    private void Inject(AudioManager audioManager)
    {
        this.audioManager = audioManager;
    }

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    private void OnButtonClick(UnityAction action)
    {
        action.Invoke();

        audioManager.Play(pressSound);

        HapticsManager.PlayHaptics(hapticsType);
    }

    public void SetupButtonEvent(UnityAction action)
    {
        button.onClick.AddListener(() => OnButtonClick(action));
    }

    public Bounds GetPanelBounds()
    {
        Vector3 rightUp = Camera.main.ScreenToWorldPoint(rect.position + new Vector3(rect.sizeDelta.x / 2, rect.sizeDelta.y / 2));
        Vector3 leftDown = Camera.main.ScreenToWorldPoint(rect.position - new Vector3(rect.sizeDelta.x / 2, rect.sizeDelta.y / 2));

        float h = Mathf.Abs(rightUp.y) - Mathf.Abs(leftDown.y);
        float w = Mathf.Abs(rightUp.x) - Mathf.Abs(leftDown.x);

        Vector3 size = new(Mathf.Abs(h), Mathf.Abs(w));

        return new Bounds(Camera.main.ScreenToWorldPoint(rect.position), size);
    } 
}
