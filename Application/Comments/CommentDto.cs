using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Comments
{
    public class CommentDto
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string body { get; set; }
        public string username { get; set; }
        public string DisplayName { get; set; }
        public string Image { get; set; }
    

    }
}