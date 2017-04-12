using System;

namespace URent.Models.Model.List
{
    public class Reservation : Model.Reservation
    {
        public string Category { get; set; }

        /// <summary>
        /// True = on peut annuler sans frais / False = frais à appliquer
        /// </summary>
        public bool CancelDelay => ((DateStartRent.Subtract(DateTime.Today)).TotalMinutes > 2880);

        public string StatusName
        {
            get
            {
                switch (Status)
                {
                    case 1:
                        return "Reserved";
                    default:
                        return "Canceled";
                }
            }
        }
    }
}
