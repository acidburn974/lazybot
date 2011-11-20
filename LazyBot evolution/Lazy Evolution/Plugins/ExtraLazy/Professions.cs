using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using LazyLib.Helpers;
using LazyLib.Wow;
using System.Threading;

namespace LazyEvo.Plugins.ExtraLazy
{
    public class Profession
    {
        public String Name { get; internal set; }
        public int Level { get; internal set; }
        public int MaxLevel { get; internal set; }
        public String Rank { get; internal set; }

        public Profession()
        {
        }
    }
    public class Professions
    {
        public Boolean IsMiner { get; internal set; }
        public Boolean IsSkinner { get; internal set; }
        public Boolean IsAlchemist { get; internal set; }
        public Boolean IsBlacksmith { get; internal set; }
        public Boolean IsEnchanter { get; internal set; }
        public Boolean IsEngineer { get; internal set; }
        public Boolean IsInscriptor { get; internal set; }
        public Boolean IsJewelCrafter { get; internal set; }
        public Boolean IsLeatherWorker { get; internal set; }
        public Boolean IsTailor { get; internal set; }
        public Boolean IsHerbalist { get; internal set; }
        public Boolean IsCook { get; internal set; }
        public Boolean IsFirstAider { get; internal set; }
        public Boolean IsArchaeologist { get; internal set; }
        public Boolean IsFisherPerson { get; internal set; }
        /// <summary>
        /// First Primary Profession
        /// </summary>
        public Profession Primary1 { get; private set; }
        /// <summary>
        /// Second Primary Profession
        /// </summary>
        public Profession Primary2 { get; private set; }
        /// <summary>
        /// First Secondary Profession useally Archaeology
        /// </summary>
        public Profession Secondary1 { get; private set; }
        /// <summary>
        /// Second Secondary Profession useally Fishing
        /// </summary>
        public Profession Secondary2 { get; private set; }
        /// <summary>
        /// Third Secondary Profession useally Cooking
        /// </summary>
        public Profession Secondary3 { get; private set; }
        /// <summary>
        /// Forth Secondary Profession useally First Aid
        /// </summary>
        public Profession Secondary4 { get; private set; }

        private Action<Professions> _callback;
        /// <summary>
        /// Creates a seperate thread that fetches the all professions and stores internally
        /// </summary>
        /// <param name="callbackMethod">A void callback method that takes one parameter of type Profession which will return itself to the callback method</param>
        public Professions(Action<Professions> callbackMethod)
        {
            IsMiner = false;
            IsSkinner = false;
            IsAlchemist = false;
            IsBlacksmith = false;
            IsEnchanter = false;
            IsEngineer = false;
            IsInscriptor = false;
            IsJewelCrafter = false;
            IsLeatherWorker = false;
            IsTailor = false;
            IsHerbalist = false;

            this._callback = callbackMethod;
            Thread workerThread = new Thread(ReloadFrames);
            workerThread.Start(this);
        }
        /// <summary>
        /// Parses chat message to check for updates to professions
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">Messages details</param>
        /// <returns>True if update occured false otherwise</returns>
        public Boolean ChatMessage(object sender, GChatEventArgs e)
        {
            Boolean unlearning = false;
            Boolean learning = false;
            if (e.Msg.Msg.Contains("You have unlearned"))
            {
                unlearning = true;
            }
            if (e.Msg.Msg.Contains("You have learned a new ability"))
            {
                learning = true;
            }
            if (unlearning || learning)
            {
                String name = e.Msg.Msg.Substring(e.Msg.Msg.IndexOf("[") + 1, e.Msg.Msg.IndexOf("]") - 1);
            }

            return false;
        }

        internal void setPrimaryProfession1(Profession profession)
        {
            this.Primary1 = profession;
        }

        internal void setPrimaryProfession2(Profession profession)
        {
            this.Primary2 = profession;
        }

        internal void setSecondaryProfession1(Profession profession)
        {
            this.Secondary1 = profession;
        }

        internal void setSecondaryProfession2(Profession profession)
        {
            this.Secondary2 = profession;
        }

        internal void setSecondaryProfession3(Profession profession)
        {
            this.Secondary3 = profession;
        }

        internal void setSecondaryProfession4(Profession profession)
        {
            this.Secondary4 = profession;
        }

        internal void callback()
        {
            this._callback(this);
        }

        internal static void ReloadFrames(Object proObj)
        {
            Professions professions = (Professions)proObj;
            InterfaceHelper.ReloadFrames();

            Thread.Sleep(2000);
            ClickHooked("SpellbookMicroButton");
            ClickHooked("SpellBookFrameTabButton2");
            List<Frame> mainFrames = new List<Frame>();
            List<Frame> statusFrames = new List<Frame>();
            mainFrames.Add(ClickHooked("PrimaryProfession1"));
            mainFrames.Add(ClickHooked("PrimaryProfession2"));
            mainFrames.Add(ClickHooked("SecondaryProfession1"));
            mainFrames.Add(ClickHooked("SecondaryProfession2"));
            mainFrames.Add(ClickHooked("SecondaryProfession3"));
            mainFrames.Add(ClickHooked("SecondaryProfession4"));
            //foreach(Frame frame in InterfaceHelper.GetFrames)
            //{
            //    Debug.WriteLine(frame.GetName);
            //}
            statusFrames.Add(ClickHooked("PrimaryProfession1StatusBar"));
            statusFrames.Add(ClickHooked("PrimaryProfession2StatusBar"));
            statusFrames.Add(ClickHooked("SecondaryProfession1StatusBar"));
            statusFrames.Add(ClickHooked("SecondaryProfession2StatusBar"));
            statusFrames.Add(ClickHooked("SecondaryProfession3StatusBar"));
            statusFrames.Add(ClickHooked("SecondaryProfession4StatusBar"));
            int x = 0;
            foreach(Frame frame in mainFrames)
            {
                List<Frame> frameChildren = frame.GetChilds;
                switch (frameChildren.ElementAt(0).GetText)
                {
                    case "Mining":
                        professions.IsMiner = true;
                        break;
                    case "Skinning":
                        professions.IsSkinner = true;
                        break;
                    case "Herbalism":
                        professions.IsHerbalist = true;
                        break;
                    case "Alchemy":
                        professions.IsAlchemist = true;
                        break;
                    case "Blacksmithing":
                        professions.IsBlacksmith = true;
                        break;
                    case "Engineering":
                        professions.IsEngineer = true;
                        break;
                    case "Enchanting":
                        professions.IsEnchanter = true;
                        break;
                    case "Inscription":
                        professions.IsInscriptor = true;
                        break;
                    case "Jewelcrafting":
                        professions.IsJewelCrafter = true;
                        break;
                    case "Leatherworking":
                        professions.IsLeatherWorker = true;
                        break;
                    case "Tailoring":
                        professions.IsTailor = true;
                        break;
                    case "Cooking":
                        professions.IsCook = true;
                        break;
                    case "Archaeology":
                        professions.IsArchaeologist = true;
                        break;
                    case "Firstaid":
                        professions.IsFirstAider = true;
                        break;
                    case "Fishing":
                        professions.IsFisherPerson = true;
                        break;
                }
                Profession profession = new Profession();
                profession.Name = frameChildren.ElementAt(0).GetText;
                int p = x > 1 ? 1 : 3;
                profession.Rank = frameChildren.ElementAt(p).GetText;
                Frame statusFrame = statusFrames.ElementAt(x);
                List<Frame> statusChildren = statusFrame.GetChilds;
                String[] levels = statusChildren.ElementAt(0).GetText.Split('/');
                profession.Level = int.Parse(levels[0]);
                profession.MaxLevel = int.Parse(levels[1]);

                switch (x)
                {
                    case 0:
                        professions.setPrimaryProfession1(profession);
                        break;
                    case 1:
                        professions.setPrimaryProfession2(profession);
                        break;
                    case 2:
                        professions.setSecondaryProfession1(profession);
                        break;
                    case 3:
                        professions.setSecondaryProfession2(profession);
                        break;
                    case 4:
                        professions.setSecondaryProfession3(profession);
                        break;
                    case 5:
                        professions.setSecondaryProfession4(profession);
                        break;
                }
                x++;
            }
            //ClickHooked("PrimaryProfession1");
            //ClickHooked("PrimaryProfession1StatusBar");
            //ClickHooked("ArchaeologyFrameSummaryPageRace4");
            Thread.Sleep(1000);
            CloseFrames();
            Thread.Sleep(1000);
            professions.callback();
        }

        private static void CloseFrames()
        {
            while (!(InterfaceHelper.GetFrameByName("GameMenuButtonContinue").IsVisible))
            {
                KeyLowHelper.PressKey(MicrosoftVirtualKeys.Escape);
                KeyLowHelper.ReleaseKey(MicrosoftVirtualKeys.Escape);
                Thread.Sleep(800);
            }
            if ((InterfaceHelper.GetFrameByName("GameMenuButtonContinue").IsVisible))
            {
                ClickHooked("GameMenuButtonContinue");
            }
        }

        private static Frame ClickHooked(string frameName)
        {
            Frame frame = null;
            try
            {
                MouseHelper.Hook();
                MouseHelper.WaitFrameReload();

                frame = InterfaceHelper.GetFrameByName(frameName);
                InterfaceHelper.GetFrameByName(frameName).LeftClickHooked();
                MouseHelper.WaitFrameReload();
                MouseHelper.ReleaseMouse();
            }
            catch (Exception)
            {
            }
            return frame;
        }
    }
}
