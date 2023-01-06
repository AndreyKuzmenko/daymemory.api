using System;

namespace DayMemory.Core.Models.Exceptions
{
    [Serializable]
    public class ConfigurationException : Exception
    {
        public string Parameter { get; set; }

        public ConfigurationException(string parameter) : base($"Configuration error. Parameter: '{parameter}'")
        {
            Parameter = parameter;
        }

    }
}
