using Module6Tp1Dojo_BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Module6Tp1Dojo.Models
{
    public class SamouraiViewModel
    {
        public Samourai Samourai { get; set; }
        public List<SelectListItem> Armes { get; set; }
        public List<SelectListItem> ArtsMartiaux { get; set; }

        public int? IdArme { get; set; }
        public List<int> IdsArtMartiaux{ get; set; }

        public double Potentiel { get; set; }
    }
}