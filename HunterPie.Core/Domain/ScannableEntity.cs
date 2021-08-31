using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HunterPie.Core.Domain
{
    //public delegate void Middleware(ref dynamic data);

    public delegate void Scan<T>();
    public delegate void Middleware<T>(ref T dto) where T : HunterPieDTO;

    public class HunterPieDTO { };

    public class HealthDTO : HunterPieDTO
    {
        public float health;
    }

    internal class Player : ScannableEntity
    {
     
        public float Health { get; private set; }

        public Player()
        {
            
        }

        private void ScanHealth()
        {

        }
    }

    internal class ScannableEntity
    {

        private readonly Dictionary<Type, HashSet<Delegate>> middlewares = new Dictionary<Type, HashSet<Delegate>>();
        private readonly Dictionary<Type, List<Action>> scanners = new Dictionary<Type, List<Action>>();
        
        protected bool AddScanner<T>(T dtoType, Action scanner) where T : HunterPieDTO
        {
            Type type = dtoType.GetType();
            if (!scanners.ContainsKey(type))
                scanners.Add(type, new List<Action>());

            scanners[type].Add(scanner);
            middlewares.Add(type, new HashSet<Delegate>());

            return true;
        }

        protected void Next<T>(ref T data) where T : HunterPieDTO
        {
            foreach (var mid in middlewares[data.GetType()])
            {
                Middleware<T> function = Delegate.CreateDelegate(
                        typeof(Middleware<T>),

                    )       
            }
            data = (T)raw;
        }

        public bool RegisterMiddleware<T>(Middleware<T> middleware) where T : HunterPieDTO
        {
            Type type = typeof(T);
            if (!middlewares.ContainsKey(type))
                return false;

            Delegate mid = Delegate.CreateDelegate(
                typeof(_Middleware),
                middleware.Target,
                middleware.Method
            );

            middlewares[type].Add(middleware);

            return true;
        }

    }
}
