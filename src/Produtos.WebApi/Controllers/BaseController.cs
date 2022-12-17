using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Produtos.Domain.Model.Enumerators;
using Produtos.Domain.Model;

namespace Produtos.WebApi.Controllers
{
    [Authorize]
    public class BaseController : ControllerBase
    {
        protected ActionResult ProcessResponse(ServiceResult result)
        {
            if (!result.Notifications.Any())
                return Ok();

            return Ok(new
            {
                Notifications = result.Notifications.Select(n => new
                {
                    Key = n.Key.ToString(),
                    Code = n.Message
                })
            });
        }

        protected ActionResult ProcessResponse<T>(ServiceResult<T> result)
        {
            return result.Status switch
            {
                ServiceResultStatus.OK => Ok(new
                {
                    Data = result.Model,
                    Notifications = result.Notifications.Select(n => new
                    {
                        Key = n.Key.ToString(),
                        Code = n.Message
                    })
                }),
                ServiceResultStatus.CREATED => Created(result.RouteLocation, null),
                ServiceResultStatus.ERROR => BadRequest(new
                {
                    Data = "error",
                    Notifications = result.Notifications.Select(n => new
                    {
                        Key = n.Key.ToString(),
                        Code = n.Message
                    })
                }),
                ServiceResultStatus.NOT_FOUND => BadRequest(new
                {
                    Data = "not_found",
                    Notifications = result.Notifications.Select(n => new
                    {
                        Key = n.Key.ToString(),
                        Code = n.Message
                    })
                }),
                _ => Problem(detail: "Case not mapped", statusCode: 500),
            };
        }
    }
}
