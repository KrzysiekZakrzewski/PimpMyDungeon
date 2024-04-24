using System.Collections.Generic;
using UnityEngine;

namespace Inputs
{
    public class InputManager : MonoBehaviour
    {
        private static Dictionary<int, PlayerInput> playersLUT;

        private void Awake()
        {
            playersLUT = new Dictionary<int, PlayerInput>();

            CreatePlayer();
        }

        private static void CreatePlayer()
        {
            PlayerInput newPlayer = new PlayerInput();

            playersLUT.Add(playersLUT.Count, newPlayer);
        }

        public static PlayerInput GetPlayer(int id)
        {
            playersLUT.TryGetValue(id, out PlayerInput player);

            return player;
        }
    }
}