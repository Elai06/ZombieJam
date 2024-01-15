using Gameplay.Tutorial;
using UnityEngine;
using Utils.ZenjectInstantiateUtil;
using Zenject;

namespace Gameplay.Windows.Tutorial
{
    public class TutorialView : MonoBehaviour
    {
        [Inject] private ITutorialService _tutorialService;

        private void Start()
        {
            InjectService.Instance.Inject(this);

            _tutorialService.СhangedState += ChangedState;
        }

        private void ChangedState(ETutorialState state)
        {
            if (state == ETutorialState.Swipe)
            {
            }
        }
    }
}