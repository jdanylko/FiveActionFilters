using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using TidyManaged;

namespace FiveActionFiltersDemo.ActionFilters
{
    public class TidyHtml : ActionFilterAttribute
    {
        private HtmlTextWriter _htmlTextWriter;
        private StringWriter _stringWriter;
        private StringBuilder _stringBuilder;
        private HttpWriter _output;

        public TidyHtml()
        {
            Xhtml = true;
            IndentBlockElements = AutoBool.Yes;
            DocType = DocTypeMode.Strict;
            XmlOut = true;
            MakeClean = true;
            HideEndTags = true;
            LogicalEmphasis = true;
            DropFontTags = true;
        }

        #region Properties

        public DocTypeMode DocType { get; set; }
        public bool DropFontTags { get; set; }
        public bool LogicalEmphasis { get; set; }
        public bool XmlOut { get; set; }
        public bool Xhtml { get; set; }
        public AutoBool IndentBlockElements { get; set; }
        public bool HideEndTags { get; set; }
        public bool MakeClean { get; set; }
        public bool TidyMark { get; set; }

        #endregion

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _stringBuilder = new StringBuilder();
            _stringWriter = new StringWriter(_stringBuilder);
            _htmlTextWriter = new XhtmlTextWriter(_stringWriter);

            _output = (HttpWriter)filterContext.RequestContext.HttpContext.Response.Output;

            filterContext.RequestContext.HttpContext.Response.Output = _htmlTextWriter;
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            var document = Document.FromString(_stringBuilder.ToString());
            document.OutputXhtml = Xhtml;
            document.IndentBlockElements = IndentBlockElements;
            document.DocType = DocTypeMode.Strict;
            document.OutputXml = XmlOut;
            document.MakeClean = MakeClean;
            document.RemoveEndTags = HideEndTags;
            document.UseLogicalEmphasis = LogicalEmphasis;
            document.DropFontTags = DropFontTags;

            document.CleanAndRepair();

            document.Save(_output.OutputStream);

        }

    }
}