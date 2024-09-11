
namespace ExtensionMethods
{
    using System;
    using System.ComponentModel;
    using System.Reflection;
    using System.Windows.Forms;

    internal static class EventHandlersToolkit
    {
        internal static void CopyEventHandlersTo(this Control src, Control dest, string eventName, bool deleteSrcHandlers)
        {
            var srcEventList = GetControlEventHandlerList(src);
            var srcEventKey = GetControlEventKey(src, eventName);
            var srcHandlers = srcEventList[srcEventKey]; //-V3080

            //Copy the srcHandlers
            var destEventList = GetControlEventHandlerList(dest);
            var destEventKey = GetControlEventKey(dest, eventName);

            destEventList.AddHandler(destEventKey, srcHandlers); //-V3080

            if (deleteSrcHandlers)
                srcEventList.RemoveHandler(srcEventKey, srcEventList[srcEventKey]);
        }

        private static object GetControlEventKey(this Control c, string eventName)
        {
            var type = c.GetType();
            var eventKeyField = TryGetStaticNonPublicFieldInfo(type, eventName);

            if (eventKeyField == null)
            {
                //Not all events in the WinForms controls use this pattern.
                //Other methods can be used to search for the event srcHandlers if required.
                return null;
            }

            return eventKeyField.GetValue(c);
        }

        private static FieldInfo TryGetStaticNonPublicFieldInfo(this Type type, string eventName)
        {
            var eventKeyField = GetStaticNonPublicFieldInfo(type, "Event" + eventName);

            if (eventKeyField == null && eventName.EndsWith("Changed"))
                eventKeyField = GetStaticNonPublicFieldInfo(type, "Event" + eventName.Remove(eventName.Length - 7)); //remove "Changed"

            if (eventKeyField == null)
                eventKeyField = GetStaticNonPublicFieldInfo(type, "EVENT_" + eventName.ToUpper());

            return eventKeyField;
        }

        //Also searches up the inheritance hierarchy
        private static FieldInfo GetStaticNonPublicFieldInfo(this Type type, string name)
        {
            FieldInfo fi;
            do
            {
                fi = type.GetField(name, BindingFlags.Static | BindingFlags.NonPublic);
                type = type.BaseType;
            } while (fi == null && type != null);

            return fi;
        }

        private static EventHandlerList GetControlEventHandlerList(this Control c)
        {
            var type = c.GetType();
            var pi = type.GetProperty("Events",
               BindingFlags.NonPublic | BindingFlags.Instance);

            return pi == null ? null : pi.GetValue(c, null) as EventHandlerList;
        }

        internal static void FireEvent(this object targetObject, string eventName, EventArgs e)
        {
            /*
             * By convention event handlers are internally called by a internal protected
             * method called OnEventName
             * e.g.
             *     internal event TextChanged
             * is triggered by
             *     internal protected void OnTextChanged
             * 
             * If the object didn't create an OnXxxx internal protected method,
             * then you're screwed. But your alternative was over override
             * the method and call it - so you'd be screwed the other way too.
             */

            //Event thrower method name //e.g. OnTextChanged
            var methodName = "On" + eventName;

            var mi = targetObject.GetType().GetMethod(
                  methodName,
                  BindingFlags.Instance | BindingFlags.NonPublic);

            _ = mi ?? throw new ArgumentException("Cannot find event thrower named " + methodName);

            mi.Invoke(targetObject, new object[] { e });
        }
    }
}