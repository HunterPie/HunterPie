using HunterPie.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using HunterPie.Core.Logger;

namespace HunterPie.Core.Extensions
{
    public static class IDispatchableExtensions
    {

        public static void Dispatch<T>(this IEventDispatcher self, EventHandler<T> toDispatch, T data)
        {
            if (toDispatch is null)
                return;

            foreach (EventHandler<T> sub in toDispatch.GetInvocationList())
            {
                try
                {
                    sub(self, data);
                } catch (Exception err)
                {
                    Log.Error($"Exception in {sub.Method.Name}:", err);
                }
            }
        }

    }
}
