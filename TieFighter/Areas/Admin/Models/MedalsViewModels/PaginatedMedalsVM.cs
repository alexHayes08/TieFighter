using System;
using System.Collections.Generic;
using TieFighter.Models;

namespace TieFighter.Areas.Admin.Models.MedalsViewModels
{
    public class PaginatedMedalsVM
    {
        public IList<Medal> Medals { get; set; }
        public DateTime LastSynced { get; set; }
    }
}
