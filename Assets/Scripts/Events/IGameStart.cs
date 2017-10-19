using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace KyleStankovich.BattleDash
{
    public interface IGameStart : IEventSystemHandler
    {
        void StartGame();
    }
}
