using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using TieFighter.Models;

namespace TieFighter.Areas.Admin.Models.MedalsViewModels
{
    public class UpdateMedalViewModel
    {
        public string Id { get; set; }
        public string MedalName { get; set; }
        public string Description { get; set; }
        public double PointsWorth { get; set; }
        public MedalCondition[] Conditions { get; set; }
        public List<SelectListItem> CondtionTypes { get; set; }
    }
}
