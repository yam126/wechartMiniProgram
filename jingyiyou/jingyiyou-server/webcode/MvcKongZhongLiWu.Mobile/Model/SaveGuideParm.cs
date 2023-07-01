using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ncc2019.Model
{
    public class SaveGuideParm
    {
        public int GuidId { get; set; }
        public string userFaceUrl { get; set; }

        public string userVideoUrl { get; set; }

        public string nickName { get; set; }

        public string PhoneNumber { get; set; }

        public string IDCard { get; set; }

        public string Intorduction { get; set; }

        public string Skills { get; set; }

        public string SkillPointsStr { get; set; }
    }
}