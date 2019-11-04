using System;

namespace Sealveru.Finances.Module
{
    public interface IApplicable
    {
        bool IsApplying { get; set; }
        bool IsApplied { get; set; }
        DateTime AppliedDate { get; set; }

        void Apply(params object[] args);

        event EventHandler Applying;
        event EventHandler Applied;
    }
}