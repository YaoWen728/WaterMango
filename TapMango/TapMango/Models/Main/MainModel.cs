using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TapMango.Domain.BusinessObjects;

namespace TapMango.Models.Main
{
    public class MainModel
    {
        public TapMangoPlantList TapMangoPlants { get; set; }

        public MainModel()
        {
            TapMangoPlants = TapMangoPlantList.GetAll();
        }
    }
}