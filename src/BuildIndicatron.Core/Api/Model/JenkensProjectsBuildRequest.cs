using System.Collections.Generic;

namespace BuildIndicatron.Core.Api.Model
{
    public class JenkensProjectsBuildRequest
    {
        public JenkensProjectsBuildRequest(params string[] values)
        {
            parameter = new List<ParameterData>();
            for (int i = 0; i < values.Length; i += 2)
            {
                var name = values[i];
                var value = (values.Length > i + 1) ? values[i + 1] : "";
                parameter.Add(new ParameterData(name, value));

            }
        }

        public List<ParameterData> parameter { get; set; }

        public class ParameterData
        {
            public ParameterData(string name, string value)
            {
                this.name = name;
                this.value = value;
            }

            public string name { get; set; }
            public string value { get; set; }
        }

    }
}