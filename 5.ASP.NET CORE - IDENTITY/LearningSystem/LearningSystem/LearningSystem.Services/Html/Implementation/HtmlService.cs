using Ganss.XSS;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearningSystem.Services.Html.Implementation
{
    public class HtmlService : IHtmlService
    {
        private readonly HtmlSanitizer htmlSanitizer;

        public HtmlService()
        {
            this.htmlSanitizer = new HtmlSanitizer();
            this.htmlSanitizer.AllowedAttributes.Add("class");
        }
        public string Sanitize(string htmlContent)
        {
          return this.htmlSanitizer.Sanitize(htmlContent);
        }
    }
}
