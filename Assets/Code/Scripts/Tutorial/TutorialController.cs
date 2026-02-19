using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XaviGames.Animation;
using XaviGames.Attributes;
using XaviGames.ObjectVariables;
using XaviGames.SaveSystem;
using XaviGames.Train;
using XaviGames.Wagon;

namespace XaviGames.Tutorial
{
    public class TutorialController : MonoBehaviour
    {
        private enum TutorialStep
        {
            Fence,
            TrafficLight,
            Completed
        }

        [SerializeField]
        private IntModel _isTutorialCompletedModel;

        [SerializeField]
        private DataController _dataController;

        [SerializeField]
        private TrainMovementController _trainMovementController;

        [SerializeField]
        private FloatVariable _trainSpeedRespawn;

        [SerializeField]
        private GameObject _tutorialArrow;

        [SerializeField]
        private SpawnAnimation _arrowSpawnAnimation;

        [SerializeField]
        private Transform _fenceTransform;

        [SerializeField]
        private Transform _trafficLightTransform;

        [SerializeField]
        private List<CapacityWagonController> _capacityWagonControllers;

        [SerializeField]
        [ReadOnly]
        private TutorialStep _stepTutorial = TutorialStep.Fence;

        [SerializeField]
        [ReadOnly]
        private int _currentLoop = 0;

        [SerializeField]
        private int _maxLoops = 2;

        private void Start()
        {
            bool isTutorialCompleted = _isTutorialCompletedModel.Value == 1;

            if (isTutorialCompleted)
            {
                _stepTutorial = TutorialStep.Completed;
                return;
            }

            _currentLoop = 0;
            FenceOpenStep();
        }

        private void Update()
        {
            if (_stepTutorial == TutorialStep.Completed)
            {
                return;
            }

            switch (_stepTutorial)
            {
                case TutorialStep.Fence:
                    {
                        CheckFenceStep();
                        break;
                    }
                case TutorialStep.TrafficLight:
                    {
                        StartCoroutine(CheckTrafficLightStep());
                        break;
                    }
            }
        }

        private void FenceOpenStep()
        {
            StartCoroutine(SetArrowState(true, _fenceTransform));
            _stepTutorial = TutorialStep.Fence;
        }

        private void TrafficLightStep()
        {
            StartCoroutine(SetArrowState(true, _trafficLightTransform));
            _stepTutorial = TutorialStep.TrafficLight;
        }

        private void CheckFenceStep()
        {
            if (_stepTutorial != TutorialStep.Fence)
            {
                return;
            }

            bool allWagonsFull = _capacityWagonControllers
                .TrueForAll(wagon => wagon.CurrentBoarded >= wagon.Capacity);

            if (allWagonsFull)
            {
                TrafficLightStep();
            }
        }

        private IEnumerator CheckTrafficLightStep()
        {
            if (_stepTutorial != TutorialStep.TrafficLight)
            {
                yield break;
            }

            if (_trainMovementController.TrainState == TrainState.Departing)
            {
                _currentLoop++;

                if (_currentLoop >= _maxLoops)
                {
                    CompleteTutorial();
                }
                else
                {
                    yield return new WaitForSeconds(_trainSpeedRespawn.Value);
                    FenceOpenStep();
                }
            }
        }

        private void CompleteTutorial()
        {
            _stepTutorial = TutorialStep.Completed;
            _isTutorialCompletedModel.SetValue(1);
            _dataController.SaveModel(_isTutorialCompletedModel);
            StartCoroutine(SetArrowState(false));
        }

        private IEnumerator SetArrowState(bool active, Transform target = null)
        {
            if (active)
            {
                _arrowSpawnAnimation.Despawn();
                yield return new WaitForSeconds(_arrowSpawnAnimation.Duration);

                if (target != null)
                {
                    _tutorialArrow.transform.position = target.position;
                }

                _arrowSpawnAnimation.Spawn();
            }
            else
            {
                _arrowSpawnAnimation.Despawn();
            }
        }
    }
}
