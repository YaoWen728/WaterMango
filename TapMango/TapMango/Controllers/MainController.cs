using System;
using System.IO;
using System.Web.Mvc;
using TapMango.Domain.Manager;
using TapMango.Domain.Validation;
using TapMango.Models.Main;

namespace TapMango.Controllers
{
    public class MainController : Controller
    {
        private MainManager MainManager { get; set; }

        public MainController()
        {
            MainManager = MainManager.GetInstance();
        }

        public ActionResult Index()
        {
            var model = new MainModel();

            return View(model);
        }

        [HttpPost]
        [WateringValidation]
        public ActionResult StartWatering(int tapMangoPlantId)
        {
            MainManager.StartWatering(tapMangoPlantId);

            return new JsonResult
            {
                Data = new
                {
                    Success = true,
                    Message = "Start watering plant!"
                }
            };
        }

        [HttpPost]
        [WateringValidation]
        public ActionResult StopWatering(int tapMangoPlantId)
        {
            MainManager.StopWatering(tapMangoPlantId);

            return new JsonResult
            {
                Data = new
                {
                    Success = true,
                    Message = "Stopped watering plant!"
                }
            };
        }
    }
}