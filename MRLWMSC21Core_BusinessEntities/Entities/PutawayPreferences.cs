using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace MRLWMSC21Core.Entities
{
    public class PutawayPreferences
    {
        private bool _StrictComplianceToSuggestions;

        public bool StrictComplianceToSuggestions { get => _StrictComplianceToSuggestions; set => _StrictComplianceToSuggestions = value; }
    }
}
