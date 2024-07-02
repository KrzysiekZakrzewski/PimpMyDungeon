using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SettingsToogle : MonoBehaviour
{
    [SerializeField]
    private Button button;
    [SerializeField]
    private Image icon;
    [SerializeField]
    private Material offMaterial;

    private bool isOn;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void OnButtonClick(UnityAction action)
    {
        action.Invoke();

        ChangeButtonState();
    }

    private void ForceChangeState(bool state)
    {
        isOn = state;

        icon.material = state ? null : offMaterial;
    }

    public void SetupButtonEvent(UnityAction action)
    {
        button.onClick.AddListener(() => OnButtonClick(action));
    }

    public void LoadButtonState(bool loadedValue)
    {
        ForceChangeState(loadedValue);
    }

    public void ChangeButtonState()
    {
        ForceChangeState(!isOn);
    }
}
