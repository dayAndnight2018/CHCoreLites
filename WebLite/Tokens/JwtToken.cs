using System;
using System.Collections.Generic;
using System.Text;

namespace WebLite.Tokens
{
    public class JwtToken
    {
        public string Uid { get; set; }

        public List<string> Role { get; set; }

        public string Name { get; set; }

    }
}
