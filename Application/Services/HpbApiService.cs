using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Services
{
    public class HpbApiService
    {
        IConfiguration Configuration { get; set; }
        public HpbApiService(IConfiguration configuration)
        {
            Configuration = configuration;
        }
    }
}
