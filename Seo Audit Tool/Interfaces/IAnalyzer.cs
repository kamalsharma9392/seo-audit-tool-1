namespace Seo_Audit_Tool.Interfaces
{
    interface IAnalyzer
    {
        bool HasKeywordInTitle();
        bool HasKeywordInDescription();
        bool[] HasKeywordInHeadings();
        void Analyze();
    }
}