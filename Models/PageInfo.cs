using System.Collections.Generic;
using System;

namespace MvcTest.Models
{	
    public class PageInfo
    {
        public int CurrentPageIndex { get; set; } 
        public int PageCount { get; set; }
        public IEnumerable<Worker> Workers { get; set; }
    }
  
}
