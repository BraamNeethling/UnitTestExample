using UnitTestExample.Interfaces;

namespace UnitTestExample
{
    public class System
    {
        private readonly ISystemService _systemService;
        public System(ISystemService systemService)
        {
            _systemService = systemService ?? throw new ArgumentNullException(nameof(systemService));
        }

        public bool DoSomething()
        {
            return _systemService.DoSomething(true);
        }
    }
}
