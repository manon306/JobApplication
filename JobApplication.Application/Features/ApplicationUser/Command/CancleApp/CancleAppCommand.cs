using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobApplication.Application.Features.ApplicationUser.Command.CancleApp
{
    public class CancleAppCommand : IRequest<Unit>
    {
        public int Id { get; set; } 
    }
}
