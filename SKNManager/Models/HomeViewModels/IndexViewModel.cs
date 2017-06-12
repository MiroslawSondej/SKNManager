using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SKNManager.Models.HomeViewModels
{
    public class IndexViewModel
    {

        public IndexViewModel()
        {
            ClubName = "Brak";
            MembersCount = 0;
            President = "Brak";
            VicePresident = "Brak";
            Secretary = "Brak";
            Treasurer = "Brak";

            ProjectCount = 0;
            ProjectInProgressCount = 0;
            EquipmentCount = 0;
            EquipmentSetsCount = 0;
            EquipmentLoansInProgressCount = 0;
            UpcomingEvent = "Brak";
        }


        // Informations

        public string ClubName { get; set; }
        public int MembersCount { get; set; }
        public string President { get; set; }
        public string VicePresident { get; set; }
        public string Secretary { get; set; }
        public string Treasurer { get; set; }



        // Statistics
        public int ProjectCount { get; set; }
        public int ProjectInProgressCount { get; set; }
        public int EquipmentCount { get; set; }
        public int EquipmentSetsCount { get; set; }
        public int EquipmentLoansInProgressCount { get; set; }
        public string UpcomingEvent { get; set; }
    }
}
