using MouseInteraction.Select;
using System;
using TimeTickSystems;

namespace GridPlacement.Hold
{
    public abstract class HoldObject : SelectObject, IHoldHandler, IBeginHoldHandler, IEndHoldHandler, IPrepeareToHoldHandler
    {
        protected event Action OnHoldE;
        protected event Action OnHoldEndE;

        protected bool isPrepeareToHold;
        protected bool isHolding;
        protected bool isLoadingHoldEvent;
        protected bool isHoldEventInvoked;

        private int tick;
        private readonly int holdToBeginTicks = 2;
        private readonly int ticksToHoldEvent = 5;

        protected override void OnEnable()
        {
            base.OnEnable();

            OnPointerDownE += OnPrepeareToHold;
            OnPointerUpE += OnEndHold;
            OnPointerExitE += OnEndHold;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            OnPointerDownE -= OnPrepeareToHold;
            OnPointerUpE -= OnEndHold;
            OnPointerExitE -= OnEndHold;
        }

        protected virtual void TimeTickSystem_OnTickPrepeareHold(object sender, OnTickEventArgs e)
        {
            if (!isPrepeareToHold)
                return;

            tick++;
            if (tick >= holdToBeginTicks)
                OnBeginHold();
        }

        private void TimeTickSystem_OnTickHold(object sender, OnTickEventArgs e)
        {
            if (!isLoadingHoldEvent || isHoldEventInvoked)
                return;

            tick++;
            if (tick >= ticksToHoldEvent)
                OnHold();
        }

        public virtual void OnBeginHold()
        {
            TimeTickSystem.OnTick -= TimeTickSystem_OnTickPrepeareHold;
            tick = 0;
            isPrepeareToHold = false;
            isHolding = true;
            isLoadingHoldEvent = true;
            TimeTickSystem.OnTick += TimeTickSystem_OnTickHold;
        }

        public void OnEndHold()
        {
            TimeTickSystem.OnTick -= TimeTickSystem_OnTickPrepeareHold;
            TimeTickSystem.OnTick -= TimeTickSystem_OnTickHold;
            tick = 0;
            isPrepeareToHold = false;
            isHolding = false;
            isHoldEventInvoked = false;
            isLoadingHoldEvent = false;
            OnHoldEndE?.Invoke();
        }

        public void OnHold()
        {
            if(isHoldEventInvoked) return;

            isHoldEventInvoked = true;
            isLoadingHoldEvent = false;

            OnHoldE?.Invoke();
        }

        public void OnPrepeareToHold()
        {
            TimeTickSystem.OnTick += TimeTickSystem_OnTickPrepeareHold;
            isPrepeareToHold = true;
        }
    }
}
