using LearningSystem.Data;
using System;
using System.ComponentModel.DataAnnotations;

namespace LearningSystem.Web.Areas.Blogg.Models.Articles
{
    public class PublishArticleFormModel
    {
        [Required]
        [MaxLength(DataConstants.ArticleTitleMaxLenght)]
        [MinLength(DataConstants.ArticleTitleMinLenght)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }
    }
}
