using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlertsApp
{
    class AlertCondition
    {

        public string parameter { get; set; }
        public string type { get; set; }
        public float value1 { get; set; }
        public float value2 { get; set; }
        public bool enable { get; set; }
        public string message { get; set; }

        public AlertCondition(string parameter, string type, float value1, float value2, bool enable, string message)
        {
            this.parameter = parameter;
            this.type = type;
            this.value1 = value1;
            this.value2 = value2;
            this.enable = enable;
            this.message = message;
        }

        public AlertCondition(string parameter, string type, float value1, bool enable, string message)
        {
            this.parameter = parameter;
            this.type = type;
            this.value1 = value1;
            this.value2 = float.NaN;
            this.enable = enable;
            this.message = message;
        }

        public override string ToString()
        {
            string enabledStatus;
            if (this.enable)
            {
                enabledStatus = "Enable";
            }
            else
            {
                enabledStatus = "Disable";
            }
            string output = "("+enabledStatus+ ") Alert when "+this.parameter.ToLower()+" is ";
            if (this.type.ToLower() == "bigger")
            {
                output += "bigger than " + this.value1.ToString();
            }else if (this.type.ToLower() == "smaller")
            {
                output += "smaller than " + this.value1.ToString();
            }
            else if (this.type.ToLower() == "equal")
            {
                output += "equal to " + this.value1.ToString();
            }
            else if (this.type.ToLower() == "between")
            {
                output += "between " + this.value1.ToString()+" and "+this.value2.ToString();
            }
            return output;
        }

        public bool isEqual(AlertCondition alertCondition)
        {
            return this.parameter == alertCondition.parameter &&
                this.type == alertCondition.type &&
                float.Equals(this.value1,alertCondition.value1) &&
                float.Equals(this.value2, alertCondition.value2);
        }
    }
}
