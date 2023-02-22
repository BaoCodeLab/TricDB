using System;
using System.Collections.Generic;
using Model.Model;

namespace Main.platform
{
    public class User
    {

        public string username { get; set; }

        public string password { get; set; }

        public string cookies { get; set; }

        public List<String> roles { get; set; }

        public List<String> permissions { get; set; }

        public PF_PROFILE PF_PROFILE { get; set; }

    }
}
