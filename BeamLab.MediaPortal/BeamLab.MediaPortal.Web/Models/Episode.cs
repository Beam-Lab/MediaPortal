using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeamLab.MediaPortal.Web.Models
{
    public class Episode
    {

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime PublishDate { get; set; }

        public bool Publish { get; set; }
        
        public string VideoLink { get; set; }

        public string PosterLink { get; set; }

    }
}
