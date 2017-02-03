using Maestrano.Api;
using Maestrano.Configuration;

namespace Maestrano.Account
{
    public class GroupRequestor : MnoClient<Group>
    {
        public GroupRequestor(Preset preset) : base(Group.IndexPath(), Group.ResourcePath(), preset)
        {
        }
    }
}
