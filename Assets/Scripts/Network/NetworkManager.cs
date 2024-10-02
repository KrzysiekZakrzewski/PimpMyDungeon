using System;
using UnityEngine;

namespace Network
{
    public class NetworkManager : MonoBehaviour
    {
        private NetworkReachability networkStatus;

        public event Action<NetworkManager> OnConected;
        public event Action<NetworkManager> OnDisconnected;

        public bool IsInitialized { private set; get; }

        public void InitializeNetwork()
        {
            if (IsInitialized)
                return;

            networkStatus = Application.internetReachability;

            IsInitialized = true;
        }

        private void Update()
        {
            if (!IsInitialized)
                return;

            CheckNetworkConnection();
        }

        private void CheckNetworkConnection()
        {
            var updateNetworkStatus = Application.internetReachability;

            if (updateNetworkStatus == networkStatus)
                return;

            ValidateStatus(updateNetworkStatus);
        }

        private void ValidateStatus(NetworkReachability updateNetworkStatus)
        {
            if(IsNetworkConnection && updateNetworkStatus != NetworkReachability.NotReachable)
            {
                networkStatus = updateNetworkStatus;
                return;
            }

            switch (updateNetworkStatus)
            {
                case NetworkReachability.NotReachable:
                    OnDisconnected?.Invoke(this);
                    break;
                case NetworkReachability.ReachableViaCarrierDataNetwork:
                    OnConected?.Invoke(this);
                    break;
                case NetworkReachability.ReachableViaLocalAreaNetwork:
                    OnConected?.Invoke(this);
                    break;
            }
        }

        public bool IsNetworkConnection => networkStatus != NetworkReachability.NotReachable;
    }
}