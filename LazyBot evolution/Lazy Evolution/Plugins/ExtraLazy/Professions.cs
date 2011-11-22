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
        public Rank Rank { get; internal set; }

        public Profession()
        {
        }
        
    }

    public static class RankFactory
    {
        private static List<Rank> ranks;

        public const int RANK_APPRENTICE = 0;
        public const int RANK_JOURNEYMAN = 1;
        public const int RANK_EXPERT = 2;
        public const int RANK_ARTISAN = 3;
        public const int RANK_MASTER = 4;
        public const int RANK_GRAND_MASTER = 5;
        public const int RANK_Illustrious = 6;
        public const int RANK_MAX = RANK_Illustrious;

        public static Rank getRank(int level)
        {
            createRanks();
            if (level <= RANK_MAX)
                return ranks.ElementAt(level);
            return null;
        }

        public static Rank getRank(String rankText)
        {
            createRanks();
            foreach (Rank rank in ranks)
            {
                if (rank.RankText.ToLower() == rankText.ToLower())
                {
                    return rank;
                }
            }
            return null;
        }

        public static Rank promote(Rank rank)
        {
            createRanks();
            if (rank.RankLevel < RANK_MAX)
            {
                return ranks.ElementAt(rank.RankLevel + 1);
            }
            return rank;
        }

        private static void createRanks()
        {
            if (ranks == null)
            {
                ranks = new List<Rank>();
                ranks.Add(new Apprentice());
                ranks.Add(new Journeyman());
                ranks.Add(new Expert());
                ranks.Add(new Artisan());
                ranks.Add(new Master());
                ranks.Add(new GrandMaster());
                ranks.Add(new Illustrious());
            }
        }
    }

    public abstract class Rank
    {
        protected String _rankText;
        protected int _minLevel;
        protected int _maxLevel;
        protected int _rankLevel;
        //TODO add rank train cost
        //TODO add minimum player level
        public String RankText { get { return this._rankText; } }
        public int MinLevel { get { return this._minLevel; } }
        public int MaxLevel { get { return this._maxLevel; } }
        public int RankLevel { get { return this._rankLevel; } }

        public static Rank operator ++(Rank c1)
        {
            return RankFactory.promote(c1);
        }
    }

    public class Apprentice : Rank
    {
        public Apprentice()
        {
            this._rankText = "Apprentice";
            this._minLevel = 0;
            this._maxLevel = 75;
            this._rankLevel = 0;
        }
    }

    public class Journeyman : Rank
    {
        public Journeyman()
        {
            this._rankText = "Journeyman";
            this._minLevel = 50;
            this._maxLevel = 150;
            this._rankLevel = 1;
        }
    }

    public class Expert : Rank
    {
        public Expert()
        {
            this._rankText = "Expert";
            this._minLevel = 125;
            this._maxLevel = 225;
            this._rankLevel = 2;
        }
    }

    public class Artisan : Rank
    {
        public Artisan()
        {
            this._rankText = "Artisan";
            this._minLevel = 200;
            this._maxLevel = 300;
            this._rankLevel = 3;
        }
    }

    public class Master : Rank
    {
        public Master()
        {
            this._rankText = "Master";
            this._minLevel = 275;
            this._maxLevel = 375;
            this._rankLevel = 4;
        }
    }

    public class GrandMaster : Rank
    {
        public GrandMaster()
        {
            this._rankText = "Grand Master";
            this._minLevel = 350;
            this._maxLevel = 450;
            this._rankLevel = 5;
        }
    }

    public class Illustrious : Rank
    {
        public Illustrious()
        {
            this._rankText = "Illustrious";
            this._minLevel = 425;
            this._maxLevel = 525;
            this._rankLevel = 6;
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

        private List<Profession> _pProfessions;
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

            this._pProfessions = new List<Profession>();

            this._callback = callbackMethod;
            Thread workerThread = new Thread(ReloadFrames);
            workerThread.Start(this);
        }

        private void parseProfession()
        {
            if (this._pProfessions.Count > 0)
            {
                this.Primary1 = this._pProfessions.ElementAt(0);
            }
            else
            {
                this.Primary1 = null;
            }
            if (this._pProfessions.Count > 1)
            {
                this.Primary2 = this._pProfessions.ElementAt(1);
            }
            else
            {
                this.Primary2 = null;
            }
        }
        /// <summary>
        /// Parses Messages to update professions maining pass in the BlueChat msg
        /// </summary>
        /// <param name="msg">String to be parsed</param>
        /// <returns>True if update occured false otherwise</returns>
        public Boolean MsgUpdate(String msg)
        {
            String name;
            if (msg.Contains("You have u"))
            {
                name = msg.Substring(msg.IndexOf("[") + 1, msg.IndexOf("]") - 1 - msg.IndexOf("["));
                return this.removeProfession(name);
                this.parseProfession();
            }
            else if (msg.Contains("You have g"))
            {
                name = msg.Substring(msg.IndexOf("he ") + 3, msg.IndexOf(" s") - msg.IndexOf("he ") - 3);
                Profession p = new Profession();
                p.Name = name;
                p.Level = 1;
                p.Rank = RankFactory.getRank(RankFactory.RANK_APPRENTICE);
                this.addProfession(p, this);
                this.parseProfession();
            }
            else if (msg.Contains("Your s"))
            {
                name = msg.Substring(msg.IndexOf("in ") + 3, msg.IndexOf(" has") - msg.IndexOf("in ") - 3);
                int start = msg.IndexOf("to ") + 3;
                int end = msg.IndexOf(".");
                int diff = end - start;
                int level = int.Parse(msg.Substring(start, diff));
                Boolean found = false;
                foreach (Profession p in this._pProfessions)
                {
                    if (p.Name == name)
                    {
                        p.Level = level;
                        found = true;
                    }
                }
                if (!found)
                {
                    switch(name)
                    {
                        case "Cooking":
                            this.Secondary3.Level = level;
                            found = true;
                            break;
                        case "Archaeology":
                            this.Secondary1.Level = level;
                            found = true;
                            break;
                        case "First Aid":
                            this.Secondary4.Level = level;
                            found = true;
                            break;
                        case "Fishing":
                            this.Secondary2.Level = level;
                            found = true;
                            break;
                    }
                }
                if (!found)
                {
                    return false;
                }
            }
            else if (msg.Contains("You have learned a new ability"))
            {
                name = msg.Substring(msg.IndexOf("[") + 1, msg.IndexOf("]") - 1 - msg.IndexOf("["));
                Profession p1 = null;
                foreach (Profession p in this._pProfessions)
                {
                    if (p.Name == name)
                    {
                        p.Rank++;
                    }
                }
                switch (name)
                {
                    case "Cooking":
                        this.Secondary3.Rank++;
                        break;
                    case "Archaeology":
                        this.Secondary1.Rank++;
                        break;
                    case "First Aid":
                        this.Secondary4.Rank++;
                        break;
                    case "Fishing":
                        this.Secondary2.Rank++;
                        break;
                }
            }
            else
            {
                return false;
            }
            return true;
        }

        internal void addPProfession(Profession profession)
        {
            this._pProfessions.Add(profession);
        }

        internal void setPrimaryProfession1(Profession profession)
        {
            this.Primary1 = profession;
            this.addPProfession(profession);
        }

        internal void setPrimaryProfession2(Profession profession)
        {
            this.Primary2 = profession;
            this.addPProfession(profession);
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
                
                Profession profession = new Profession();
                profession.Name = frameChildren.ElementAt(0).GetText;
                int p = x > 1 ? 1 : 3;
                profession.Rank = RankFactory.getRank(frameChildren.ElementAt(p).GetText);
                Frame statusFrame = statusFrames.ElementAt(x);
                List<Frame> statusChildren = statusFrame.GetChilds;
                String[] levels = statusChildren.ElementAt(0).GetText.Split('/');
                profession.Level = int.Parse(levels[0]);
                
                professions.addProfession(profession, professions);   
                x++;
            }
            //ClickHooked("PrimaryProfession1");
            //ClickHooked("PrimaryProfession1StatusBar");
            //ClickHooked("ArchaeologyFrameSummaryPageRace4");
            professions.parseProfession();
            Thread.Sleep(1000);
            CloseFrames();
            Thread.Sleep(1000);
            professions.callback();
        }

        internal void addProfession(Profession profession, Professions professions)
        {
            if (profession.Name != "")
            {
                switch (profession.Name)
                {
                    case "Mining":
                        professions.addPProfession(profession);
                        break;
                    case "Skinning":
                        professions.addPProfession(profession);
                        break;
                    case "Herbalism":
                        professions.addPProfession(profession);
                        break;
                    case "Alchemy":
                        professions.addPProfession(profession);
                        break;
                    case "Blacksmithing":
                        professions.addPProfession(profession);
                        break;
                    case "Engineering":
                        professions.addPProfession(profession);
                        break;
                    case "Enchanting":
                        professions.addPProfession(profession);
                        break;
                    case "Inscription":
                        professions.addPProfession(profession);
                        break;
                    case "Jewelcrafting":
                        professions.addPProfession(profession);
                        break;
                    case "Leatherworking":
                        professions.addPProfession(profession);
                        break;
                    case "Tailoring":
                        professions.addPProfession(profession);
                        break;
                    case "Cooking":
                        professions.Secondary3 = profession;
                        break;
                    case "Archaeology":
                        professions.Secondary1 = profession;
                        break;
                    case "First Aid":
                        professions.Secondary4 = profession;
                        break;
                    case "Fishing":
                        professions.Secondary2 = profession;
                        break;
                }
            }
        }

        internal Boolean removeProfession(String name)
        {
            Boolean found = false;
            Profession remove = null;
            foreach (Profession p in this._pProfessions)
            {
                if (p.Name == name)
                {
                    remove = p;
                    found = true;
                }
            }
            if (found)
            {
                this._pProfessions.Remove(remove);
            }
            else
            {
                switch (name)
                {
                    case "Cooking":
                        this.Secondary3 = null;
                        found = true;
                        break;
                    case "Archaeology":
                        this.Secondary1 = null;
                        found = true;
                        break;
                    case "First Aid":
                        this.Secondary4 = null;
                        found = true;
                        break;
                    case "Fishing":
                        this.Secondary2 = null;
                        found = true;
                        break;
                }
            }
            this.parseProfession();
            return found;
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
