﻿using System;
using LazyLib;
using LazyLib.Helpers;
using LazyLib.IPlugin;
using LazyEvo.Plugins.ExtraLazy;
using System.Windows.Forms;
using System.Threading;

namespace LazyEvo.Plugins.LazyData 
{
    public class Loader : ILazyPlugin
    {
        private static Professions professions;
        private static frmProfessions settingsForm;
        private static Boolean professionsLoaded;

        public string GetName()
        {
            return "LazyData Demo";
        }

        public void PluginLoad()
        {
            //creating a static instance to the form we don't need anymore
            settingsForm = new frmProfessions();
            professionsLoaded = false;
            
            Logging.Write("LazyData demo started");
            Chat.NewChatMessage += ChatNewChatMessage;
            
        }
        /**
         * Call back function for when the professions class has been populated
         **/
        public static void professionsReady(Professions p)
        {
            //setting the professions to the form
            settingsForm.Professions = p;
            //telling the form to change the labels to the returned results
            settingsForm.createDisplay();
            //creating a local reference to the professions
            professions = p;
            professionsLoaded = true;
        }

        public void PluginUnload()
        {
            Logging.Write("LazyData Demo stopped");
            Chat.NewChatMessage -= ChatNewChatMessage;
        }

        public void BotStart()
        {
        }

        public void BotStop()
        {
        }

        public void Pulse()
        {
        }

        public static void getProfessions()
        {
            //creates a callback function to return professions when it is finished
            Action<Professions> callback = delegate(Professions p) { professionsReady(p); };

            //the constructor on Professions with automatically populate itself
            Professions professions = new Professions(callback);
        }

        public void Settings()
        {
            getProfessions();
            Logging.Write("Viewing professions");
            settingsForm.Show();
        }

        private void ChatNewChatMessage(object sender, GChatEventArgs e)
        {
            if (professionsLoaded)
            {
                professions.ChatMessage(sender, e);
            }
            //Logging.Write("Plugin got chat message: " + e.Msg.Player + " " + e.Msg.Msg);
        }        
    }

    public class generalChat : Chat
    {
        public generalChat()
        {
        }


    }
}
