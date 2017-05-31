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