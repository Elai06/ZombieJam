using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Infrastructure.Timer
{
    public class TimerService : MonoBehaviour
    {
        private const int SECOND = 1;

        public Dictionary<string, TimeModel> TimeModels { get; set; } = new();

        private float _timer;

        public TimeModel CreateTimer(string id, int duration)
        {
            if (TimeModels.TryGetValue(id, out var timeModel) && timeModel.IsWork)
            {
                return timeModel;
            }

            var progress = new TimeProgress(id, duration);
            var model = new TimeModel(progress, duration);
            TimeModels.Add(id, model);
            return model;
        }

        private void OnEnable()
        {
            foreach (var model in TimeModels.Values)
            {
                model.Stopped += OnStop;
            }
        }

        private void OnDisable()
        {
            foreach (var model in TimeModels.Values)
            {
                model.Stopped -= OnStop;
            }
        }

        private void Update()
        {
            _timer += Time.unscaledDeltaTime;

            if (_timer >= SECOND)
            {
                for (int i = 0; i < TimeModels.Count; i++)
                {
                    TimeModels.Values.ToList()[i].AboutTick();
                }

                _timer = 0;
            }
        }

        public void OnStop(TimeModel model)
        {
            model.Stopped -= OnStop;
            TimeModels.Remove(model.TimeProgress.ID);
        }
    }
}