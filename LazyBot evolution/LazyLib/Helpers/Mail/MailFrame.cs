
﻿/*
This file is part of LazyBot - Copyright (C) 2011 Arutha

    LazyBot is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    LazyBot is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with LazyBot.  If not, see <http://www.gnu.org/licenses/>.
*/

 using System;
using System.Threading;

namespace LazyLib.Helpers.Mail
{
    internal class MailFrame
    {
        public static Boolean Open
        {
            get
            {
                try
                {
                    return InterfaceHelper.GetFrameByName("MailFrame").IsVisible;
                }
                catch
                {
                    return false;
                }
            }
        }

        public static void ClickMailFrame()
        {
            try
            {
                InterfaceHelper.GetFrameByName("MailFrame").LeftClick();
            } catch {}
        }

        public static String GetReciver
        {
            get
            {
                try
                {
                    return InterfaceHelper.GetFrameByName("SendMailNameEditBox").GetEditBoxText;
                }
                catch
                {
                    return String.Empty;
                }
            }
        }

        public static String GetMailSubject
        {
            get
            {
                try
                {
                    return InterfaceHelper.GetFrameByName("SendMailSubjectEditBox").GetEditBoxText;
                }
                catch
                {
                    return String.Empty;
                }
            }
        }

        public static Boolean CurrentTabIsSendMail
        {
            get
            {
                try
                {
                    return InterfaceHelper.GetFrameByName("SendMailAttachment1").IsVisible;
                }
                catch
                {
                    return false;
                }
            }
        }

        public static Boolean CurrentTabIsInbox
        {
            get
            {
                try
                {
                    return InterfaceHelper.GetFrameByName("InboxPrevPageButton").IsVisible;
                }
                catch
                {
                    return false;
                }
            }
        }

        public static void Close()
        {
            try
            {
                while (Open)
                {
                    InterfaceHelper.GetFrameByName("InboxCloseButton").LeftClick();
                    Thread.Sleep(250);
                }
            }
            catch
            {
            }
        }

        public static void SetMailModeSendMoney()
        {
            try
            {
                InterfaceHelper.GetFrameByName("SendMailSendMoneyButton").LeftClick();
            }
            catch
            {
            }
        }

        public static void SetMailModeCod()
        {
            try
            {
                InterfaceHelper.GetFrameByName("SendMailCODButton").LeftClick();
            }
            catch
            {
            }
        }

        public static void SetReceiver(String receiver)
        {
            Frame f = InterfaceHelper.GetFrameByName("SendMailNameEditBox");
            if (f != null)
            {
                f.SetEditBoxText(receiver);
            }
        }

        public static void SetReceiverHooked(String receiver)
        {
            Frame f = InterfaceHelper.GetFrameByName("SendMailNameEditBox");
            if (f != null)
            {
                f.SetEditBoxTextHooked(receiver);
            }
        }

        public static void SetMailSubject(String subject)
        {
            Frame f = InterfaceHelper.GetFrameByName("SendMailSubjectEditBox");
            if (f != null)
            {
                f.SetEditBoxText(subject);
            }
        }

        public static void SetMailBody(String body)
        {
            Frame f = InterfaceHelper.GetFrameByName("SendMailBodyEditBox");
            if (f != null)
            {
                f.SetEditBoxText(body);
            }
        }

        public static void ClickInboxTab()
        {
            try
            {
                InterfaceHelper.GetFrameByName("MailFrameTab1").LeftClick();
            }
            catch
            {
            }
        }

        public static void ClickSendMailTab()
        {
            try
            {
                InterfaceHelper.GetFrameByName("MailFrameTab2").LeftClick();
            }
            catch
            {
            }
        }

        public static void ClickSendMailTabHooked()
        {
            try
            {
                InterfaceHelper.GetFrameByName("MailFrameTab2").LeftClickHooked();
            }
            catch
            {
            }
        }

        public static void ClickSend()
        {
            try
            {
                ClickMailFrame();
                Thread.Sleep(1200);
                InterfaceHelper.GetFrameByName("SendMailMailButton").LeftClick();
            }
            catch
            {
            }
        }

        public static void ClickSendHooked()
        {
            try
            {
                InterfaceHelper.GetFrameByName("SendMailMailButton").LeftClickHooked();
            }
            catch
            {
            }
        }
    }
}