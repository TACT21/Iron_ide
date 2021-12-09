using Microsoft.AspNetCore.Components;
using System.Text.RegularExpressions;
using Syncfusion.Blazor.RichTextEditor;

namespace Ide.scripts
{
    public partial class Seth: ComponentBase
    {
        public string inline = string.Empty;
        public string molding =string.Empty;
        public void Mold()
        {
            molding = Regex.Replace(inline, "<[^>]*?>", "");
        }
    }
}
