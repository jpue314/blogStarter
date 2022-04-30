using System;
using System.Collections.Generic;
using JPFinalProject.Models;
using System.ComponentModel.DataAnnotations;


    public class Blog {
        public int BlogId { get; set; }
        public string BlogTitle { get; set; }
        public string BlogContent { get; set; }
        public AppUser? User { get; set; }
        public string BlogTimeStamp { get; set; }
    }
