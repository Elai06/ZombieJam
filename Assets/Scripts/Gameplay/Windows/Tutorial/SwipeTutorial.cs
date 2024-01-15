using DG.Tweening;
using Gameplay.Tutorial;
using Infrastructure.Windows;
using UnityEngine;
using Utils.ZenjectInstantiateUtil;
using Zenject;

namespace Gameplay.Windows.Tutorial
{
    public class SwipeTutorial : MonoBehaviour
    {
        [Inject] private ITutorialService _tutorialService;
        [Inject] private IWindowService _windowService;

        private void Start()
        {
            InjectService.Instance.Inject(this);

            StartAnimation();
        }

        private void StartAnimation()
        {
            transform.DOLocalMoveZ(transform.position.z - 1.5f, 1f).SetLoops(-1, LoopType.Yoyo);
        }
    }
}