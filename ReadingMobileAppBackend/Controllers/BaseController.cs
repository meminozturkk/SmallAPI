using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebAPI.Controllers;

    public class BaseController : ControllerBase
    {
        protected IMediator Mediator =>
            _mediator ??=
                HttpContext.RequestServices.GetService<IMediator>()
                ?? throw new InvalidOperationException("IMediator cannot be retrieved from request services.");

        private IMediator? _mediator;

        protected string getIpAddress()
        {
            string ipAddress = Request.Headers.ContainsKey("X-Forwarded-For")
                ? Request.Headers["X-Forwarded-For"].ToString()
                : HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString()
                    ?? throw new InvalidOperationException("IP address cannot be retrieved from request.");
            return ipAddress;
        }

        protected int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                throw new UnauthorizedAccessException("Kullanıcı girişi yapılmamış veya geçersiz token.");
            }
            return userId;
        }
    }

