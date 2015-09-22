using System.Linq;
using System.Web.Http;
using RedirectService.Models;
using Sinch.ServerSdk;
using Sinch.ServerSdk.Calling.Models;
using Sinch.ServerSdk.Models;


namespace RedirectService.Controllers {
    public class SinchController : ApiController {
        [HttpPost]
        public SvamletModel Post(CallbackEventModel model) {
            var sinch = SinchFactory.CreateCallbackResponseFactory(Locale.EnUs);
            SvamletModel result = null;
            var builder = sinch.CreateIceSvamletBuilder();
            if (NumberConfigContext.Current().Any(c => c.From == model.From)) {
                var config = NumberConfigContext.Current().FirstOrDefault(c => c.From == model.From);
                result = builder.ConnectPstn(config.To).WithCli(model.To.Endpoint).WithoutCallbacks().Model;
            } else {
                result = builder.Say("Invalid caller id!").Hangup().Model;
            }
            return result;
        }
    }
}
