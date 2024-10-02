
namespace Haptics
{
    public static class HapticsManager
    {
        private static bool hapticsOn = true;

        public static void Init()
        {
            Vibration.Init();
        }

        public static void Load(bool value)
        {
            hapticsOn = value;
        }

        public static bool OnOffHaptics()
        {
            hapticsOn = !hapticsOn;

            return hapticsOn;
        }

        public static void PlayHaptics(HapticsType hapticsType)
        {
            if (!hapticsOn || hapticsType == HapticsType.None)
                return;

            switch (hapticsType)
            {
                case HapticsType.Standard:
                    Vibration.Vibrate();
                    break;
                case HapticsType.Peek:
                    Vibration.VibratePeek();
                    break;
                case HapticsType.Pop:
                    Vibration.VibratePop();
                    break;
            }
        }
    }

    public enum HapticsType
    {
        None,
        Standard,
        Peek,
        Pop
    }
}