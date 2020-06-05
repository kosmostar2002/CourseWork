using System;
using System.Collections.Generic;
using System.Text;

namespace Facylity_
{
    // Клас, що відповідає за спеціальність
    class Speciality
    {
        internal string SpecialityName_;
        internal List<Flow> Flows = new List<Flow>();

        public Speciality(string SpecialityName)
        {
            SpecialityName_ = SpecialityName;
        }

        public void AddFlow(string flowName, int course)
        {
            Flows.Add(new Flow(flowName, course));
        }
    }
}
