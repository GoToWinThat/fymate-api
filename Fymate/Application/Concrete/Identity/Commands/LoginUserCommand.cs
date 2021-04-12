using Core.Base.Interfaces;
using Core.Concrete.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Concrete.Identity.Commands
{
    public class LoginUserCommand : IRequest<JWTAuthorizationResult>
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }

    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, JWTAuthorizationResult>
    {
        private readonly IIdentityService _identityService;

        public LoginUserCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<JWTAuthorizationResult> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var result = await _identityService.LoginUserAsync(request.Email, request.Password);
            return result;
        }
    }
}
