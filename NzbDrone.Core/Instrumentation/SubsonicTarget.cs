﻿using System;
using NLog;
using NLog.Targets;
using SubSonic.Repository;

namespace NzbDrone.Core.Instrumentation
{
    public class SubsonicTarget : Target
    {
        private readonly IRepository _repo;

        public SubsonicTarget(IRepository repo)
        {
            _repo = repo;
        }

        protected override void Write(LogEventInfo logEvent)
        {
            var log = new Log();
            log.Time = logEvent.TimeStamp;
            log.Message = logEvent.FormattedMessage;

            if (logEvent.UserStackFrame != null)
            {
                log.Method = logEvent.UserStackFrame.GetMethod().Name;
            }



            log.Logger = logEvent.LoggerName;

            if (logEvent.Exception != null)
            {
                if (String.IsNullOrWhiteSpace(log.Message))
                    log.Message = logEvent.Exception.Message;

                log.Exception = logEvent.Exception.ToString();
                log.ExceptionType = logEvent.Exception.GetType().ToString();
            }


            log.Level = logEvent.Level.Name;


            _repo.Add(log);
        }
    }
}