using Core.Base.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core.UseCases.Identity.Commands
{
    public class LoginUserCommand : IRequest<bool>
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }

    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, bool>
    {
        private readonly IIdentityService _identityService;

        public LoginUserCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<bool> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var result = await _identityService.LoginUserAsync(request.Email, request.Password);
            return result.Succeeded;
        }
    }
}
