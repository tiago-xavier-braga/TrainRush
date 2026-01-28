using System.Collections.Generic;
using UnityEngine;

namespace XaviGames.Interaction
{
    public class BoardingQueueController : MonoBehaviour
    {
        [SerializeField]
        private List<WagonQueueController> _wagonQueueController = new List<WagonQueueController>();

        public void AddCharacter(GameObject Character)
        {
            WagonQueueController wagonQueueController = null;
            foreach (WagonQueueController wagonQueue in _wagonQueueController)
            {
                if (wagonQueue.IsLastPositionEmpty() && wagonQueue.IsWagonEmpty())
                {
                    wagonQueueController = wagonQueue;
                    break;
                }
            }

            if (wagonQueueController == null)
            {
                wagonQueueController = _wagonQueueController.Find(wagonQueue => wagonQueue.IsLastPositionEmpty());
            }

            wagonQueueController?.AddCharacter(Character);
        }
    }
}
