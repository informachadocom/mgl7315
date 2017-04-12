using System;

namespace URent.Models.Model.List
{
    public class Rent : Model.Rent
    {
        public string Category { get; set; }

        public string StatusName
        {
            get
            {
                switch (Status)
                {
                    case 1:
                        return "Confirmed";
                    default:
                        return "Canceled";
                }
            }
        }
    }
}
