using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContainerManagementSystem.Models
{
    public static class Roles
    {
        public const string Administrator = "Administrator";

        public const string Agent = "Agent";

        public const string AdministratorOrAgent = Administrator + "," + Agent;
    }
}
