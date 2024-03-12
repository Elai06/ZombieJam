using System;
using Infrastructure.PersistenceProgress;

namespace Infrastructure.Session
{
    public class SessionManager : ISessionManager
    {
        private readonly IProgressService _progressService;

        private DateTime _startSessionDateTime;
        private SessionProgress _sessionProgress;

        public SessionManager(IProgressService progressService)
        {
            _progressService = progressService;
            if (!progressService.IsLoaded)
            {
                progressService.OnLoaded += Loaded;
            }
            else
            {
                Loaded();
            }
        }

        private void Loaded()
        {
            _sessionProgress = _progressService.PlayerProgress.SessionProgress;
            _startSessionDateTime = DateTime.Now;
            _sessionProgress.SessionNumber++;
        }

        public void SendSessionEvent()
        {
            var sessionTime = (DateTime.Now - _startSessionDateTime).Minutes;
            _sessionProgress.SessionsDuration += sessionTime;
            var averageTimeSessions = AverageTimeSessions();
            var parameters =
                $"{{\"time\":\"{_sessionProgress.SessionsDuration}\", " +
                $"\"session_number\":\"{_sessionProgress.SessionNumber}\", " +
                $"\"session_duration\":\"{sessionTime}\"}}";
            AppMetrica.Instance.ReportEvent("playtime_min", parameters);
        }

        private int AverageTimeSessions()
        {
            return _sessionProgress.SessionsDuration / _sessionProgress.SessionNumber;
        }
    }
}