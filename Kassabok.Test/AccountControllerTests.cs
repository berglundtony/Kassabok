using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kassabok.Test
{
    public class AccountControllerTests: IClassFixture<ApiWebApplicationFactory<TestStartup>>
    {
        readonly HttpClient client;

        public AccountControllerTests(ApiWebApplicationFactory<TestStartup> application)
        {
            _client = application.CreateClient();
        }
    }
}
