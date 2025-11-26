using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XaviGames.Attributes;

namespace XaviGames.Tasks
{
    public class TaskPriorityController : MonoBehaviour
    {
        [field: SerializeField]
        [field: ReadOnly]
        public GameTask CurrentHighestPriorityTask { get; private set; } = null;

        [Space(8f)]
        [SerializeField]
        private List<GameTask> _allGameTasks = new();

        [SerializeField]
        [ReadOnly]
        private List<GameTask> _availableTasks = new();

        private void Update()
        {
            for (int i = 0; i < _availableTasks.Count; i++)
            {
                if (!_availableTasks[i].IsAvailable)
                {
                    _availableTasks.RemoveAt(i);
                    i--;
                }
            }

            foreach (GameTask task in _allGameTasks)
            {
                if (!task.IsAvailable)
                {
                    continue;
                }

                if (!_availableTasks.Contains(task))
                {
                    _availableTasks.Add(task);
                }
            }

            if (CurrentHighestPriorityTask != null)
            {
                if (CurrentHighestPriorityTask.IsAvailable)
                {
                    return;
                }
            }

            if (_availableTasks.Count == 0)
            {
                CurrentHighestPriorityTask = null;
                return;
            }

            CurrentHighestPriorityTask = _availableTasks.First();
        }
    }
}
