﻿namespace CyberPlatGate.Contracts.Gate
{
    /// <summary>Represents generic error with description and optional code.</summary>
    public class Error
    {
        public int? Code { get; set; }
        public string Description { get; set; }

        public override string ToString()
        {
            return $"{Description} (code {Code?.ToString() ?? "null"})";
        }
    }
}
