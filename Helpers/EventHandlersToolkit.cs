
namespace MusicBeePlugin
{
    using System;
    using System.ComponentModel;
    using System.Reflection;
    using System.Windows.Forms;

    internal static class EventHandlersToolkit
    {
        internal static void CopyEventHandlers(Control src, Control dest, string eventName, bool deleteSrcHandlers)
        {
            var srcEventList = GetControlEventHandlerList(src);
            var srcEventKey = GetControlEventKey(src, eventName);
            var srcHandlers = srcEventList[srcEventKey];

            // Copy the srcHandlers
            var destEventList = GetControlEventHandlerList(dest);
            var destEventKey = GetControlEventKey(dest, eventName);

            destEventList.AddHandler(destEventKey, srcHandlers);

            if (deleteSrcHandlers)
                srcEventList.RemoveHandler(srcEventKey, srcEventList[srcEventKey]);
        }

        private static object GetControlEventKey(Control c, string eventName)
        {
            Type type = c.GetType();
            FieldInfo eventKeyField = TryGetStaticNonPublicFieldInfo(type, eventName);

            if (eventKeyField == null)
            {
                // Not all events in the WinForms controls use this pattern.
                // Other methods can be used to search for the event srcHandlers if required.
                return null;
            }

            return eventKeyField.GetValue(c);
        }

        private static FieldInfo TryGetStaticNonPublicFieldInfo(Type type, string eventName)
        {
            FieldInfo eventKeyField = GetStaticNonPublicFieldInfo(type, "Event" + eventName);

            if (eventKeyField == null && eventName.EndsWith("Changed"))
                eventKeyField = GetStaticNonPublicFieldInfo(type, "Event" + eventName.Remove(eventName.Length - 7)); // remove "Changed"

            if (eventKeyField == null)
                eventKeyField = GetStaticNonPublicFieldInfo(type, "EVENT_" + eventName.ToUpper());

            return eventKeyField;
        }

        // Also searches up the inheritance hierarchy
        private static FieldInfo GetStaticNonPublicFieldInfo(Type type, string name)
        {
            FieldInfo fi;
            do
            {
                fi = type.GetField(name, BindingFlags.Static | BindingFlags.NonPublic);
                type = type.BaseType;
            } while (fi == null && type != null);

            return fi;
        }

        private static EventHandlerList GetControlEventHandlerList(Control c)
        {
            Type type = c.GetType();
            PropertyInfo pi = type.GetProperty("Events",
               BindingFlags.NonPublic | BindingFlags.Instance);

            return (EventHandlerList)pi.GetValue(c, null);
        }

        internal static void FireEvent(object targetObject, string eventName, EventArgs e)
        {
            /*
             * By convention event handlers are internally called by a protected
             * method called OnEventName
             * e.g.
             *     internal event TextChanged
             * is triggered by
             *     protected void OnTextChanged
             * 
             * If the object didn't create an OnXxxx protected method,
             * then you're screwed. But your alternative was over override
             * the method and call it - so you'd be screwed the other way too.
             */

            //Event thrower method name //e.g. OnTextChanged
            string methodName = "On" + eventName;

            MethodInfo mi = targetObject.GetType().GetMethod(
                  methodName,
                  BindingFlags.Instance | BindingFlags.NonPublic);

            if (mi == null)
                throw new ArgumentException("Cannot find event thrower named " + methodName);

            mi.Invoke(targetObject, new object[] { e });
        }
    }
}