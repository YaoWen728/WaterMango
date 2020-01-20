using System;
using System.Diagnostics;
using System.Linq;
using TapMango.Domain.BusinessObjects;
using TapMango.Models.Main;

namespace TapMango.Domain.Manager
{
    public class MainManager
    {
        private static MainManager _instance;

        private MainManager()
        {
        }

        public static MainManager GetInstance()
        {
            return _instance ?? (_instance = new MainManager());
        }

        public void StartWatering(int tapMangoPlantId)
        {
            var now = DateTime.Now;
            //var timeRemaining = 0m;
            var tapMangoPlant = TapMangoPlant.GetById(tapMangoPlantId);
            var currentSession = tapMangoPlant.Sessions.FirstOrDefault();

            if (currentSession == null)
            {
                CreateNewSession(tapMangoPlant.Id, now);
                //timeRemaining = 10000;
            }
            else if (now > currentSession.ExpectToCompleteOn && currentSession.StatusId == 1)
            {
                if (currentSession.ExpectToCompleteOn.AddSeconds(30) > now)
                    throw new Exception("Plant need to rest!");

                CreateNewSession(tapMangoPlant.Id, now);
                //timeRemaining = 10000;
            }
            else
            {
                if (currentSession.StatusId == 1)
                    throw new Exception("You are already watering this plant!");

                var difference = currentSession.ExpectToCompleteOn - currentSession.StartTime;

                currentSession.StatusId = 1;
                currentSession.StartTime = now;
                currentSession.ExpectToCompleteOn = now + difference;
                currentSession.Update();

                //timeRemaining = Convert.ToDecimal(difference.Ticks) / 10000;
            }

            //return timeRemaining;
        }

        public void CreateNewSession(int tapMangoPlantId, DateTime now)
        {
            var newSession = WateringSession.NewSession();

            newSession.TapMangoPlantId = tapMangoPlantId;
            newSession.StatusId = 1;
            newSession.StartTime = now;
            newSession.ExpectToCompleteOn = now.AddSeconds(10);
            newSession.CreatedOn = now;
            newSession.Insert();
        }

        public void StopWatering(int tapMangoPlantId)
        {
            var now = DateTime.Now;

            var tapMangoPlant = TapMangoPlant.GetById(tapMangoPlantId);
            var currentSession = tapMangoPlant.Sessions.First();

            if (currentSession == null)
                throw new Exception("Can't stop when you are not watering plant!");
            if (currentSession.StatusId == 2 || now > currentSession.ExpectToCompleteOn)
                throw new Exception("Can't stop when you are not watering plant!");

            currentSession.StatusId = 2;
            currentSession.StartTime = now;
            currentSession.Update();
        }
    }
}