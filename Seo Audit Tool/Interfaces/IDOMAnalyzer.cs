using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seo_Audit_Tool.Interfaces
{
    interface IDomAnalyzer
    {
        bool HasKeywordInTitle();
        bool HasKeywordInDescription();
        bool[] HasKeywordInHeadings();
        void Analyze();

    }
}
