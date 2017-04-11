using System;

namespace URent.Models.Model.List
{
    public class Reservation : Model.Reservation
    {
        public string Category { get; set; }

        /// <summary>
        /// True = on peut annuler sans frais / False = frais à appliquer
        /// </summary>
        public bool CancelDelay => ((DateTime.Today.Subtract(DateStartRent)).Minutes < 2880);
    }
}
