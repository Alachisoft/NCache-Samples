using Alachisoft.NCache.Runtime.Dependencies;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alachisoft.NCache.Samples
{
    public class DependencyChangeCorrelator
    {
        private Hashtable _dependencies;
        public DependencyChangeCorrelator()
        {
            _dependencies = new Hashtable();
        }
        public static DependencyChangeCorrelator Instance { get; private set; } = new DependencyChangeCorrelator();

        public void RegisterDependency(string mappingKey, NotifyExtensibleDependency dependency)
        {
            lock (_dependencies)
            {
                _dependencies.Add(mappingKey, dependency);
            }
        }

        public bool UnRegisterDependency(string mappingKey)
        {
            lock (_dependencies)
            {
                _dependencies.Remove(mappingKey);
                if (_dependencies.Count < 1)
                    return true;
            }
            return false;

        }

        public void OnFeedChange(params object[] args)
        {
            //find the matching dependency and fire it's DependencyChangedEvent
            var key = args[0];
            var notifyExtensibleDep = (_dependencies[key] as NotifyExtensibleDependency);
            notifyExtensibleDep?.DependencyChanged.Invoke(this);
            
            lock(_dependencies)
                _dependencies.Remove(key);

        }

    }
}
