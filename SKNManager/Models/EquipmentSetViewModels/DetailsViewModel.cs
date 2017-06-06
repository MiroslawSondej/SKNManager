using SKNManager.Models.EquipmentViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SKNManager.Models.EquipmentSetViewModels
{
    public class DetailsViewModel
    {
        public Equipment [] Equipment { get; set; }
        public EquipmentSet EquipmentSet { get; set; }
    }
}
