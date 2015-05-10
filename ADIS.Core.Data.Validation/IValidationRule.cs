using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Data.Validation
{
    public interface IValidationRule
    {
        void AddRule(IChainedValidationRule rule);
        void SwapRule(IChainedValidationRule original, IChainedValidationRule replacement);
        void RemoveRule(IChainedValidationRule original);
        void SetContinuation(ContinuationStyle style);
        IChainedValidationRule CurrentRule
        {
            get;
        }
    }
}
