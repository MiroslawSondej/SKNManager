using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SKNManager.Utils.Identity
{
    public static class ClubRolesFactory
    {
        public enum Role
        {
            SUPERVISOR,
            PRESIDENT,
            VICE_PRESIDENT,
            SECRETARY,
            TREASURER,
            PHOTOGRAPHER,
            MEMBER,
            NONE,
        }

        static Dictionary<Role, string> roles = new Dictionary<Role, string>() {
            { Role.SUPERVISOR, "Opiekun" },
            { Role.PRESIDENT, "Przewodniczący" },
            { Role.VICE_PRESIDENT, "Zastępca przewodniczącego" },
            { Role.SECRETARY, "Sekretarz" },
            { Role.TREASURER, "Skarbnik" },
            { Role.PHOTOGRAPHER, "Fotograf" },
            { Role.MEMBER, "Członek" },
            { Role.NONE, "Brak" },
        };

        public static string GetName(Role role)
        {
            if (!roles.ContainsKey(role))
                return roles[Role.NONE];

            return roles[role];
        }
        public static Role GetId(string name)
        {
            if (!roles.ContainsValue(name))
                return Role.NONE;

            try
            {
                KeyValuePair<Role, string>[] role = roles.Where<KeyValuePair<Role, string>>(r => r.Value == name).ToArray<KeyValuePair<Role, string>>();
                if (role == null)
                    return Role.NONE;

                return role[0].Key;
            } catch (ArgumentNullException) {  }

            return Role.NONE;
        }
        public static string[] GetAll()
        {
            if (roles.Values.Count > 0)
                return roles.Values.ToArray();
            else
                return new string[] { "Brak" };
        }
    }
}
