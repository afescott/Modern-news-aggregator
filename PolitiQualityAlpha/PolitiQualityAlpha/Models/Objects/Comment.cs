using System;
using System.Collections.Generic;
using System.Text;

namespace PolitiQualityAlpha.Logic.Objects
{
    public class Comment
    {

        public int CommentId { get; set; }

        public string CommentText { get; set; }

        public int UserId { get; set; }

        public DateTime CommentDate { get; set; }

        public int ArticleSetId { get; set; }



    }
}
