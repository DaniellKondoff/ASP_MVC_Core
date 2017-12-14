using System;
using System.Collections.Generic;
using System.Text;

namespace LearningSystem.Services.Contracts
{
    public interface IPdfGenerator
    {
        byte[] GeneratePdfGromHtml(string html);
    }
}
