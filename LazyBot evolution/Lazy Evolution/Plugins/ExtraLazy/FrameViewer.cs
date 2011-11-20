using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LazyLib.Helpers;

namespace LazyEvo.Plugins.ExtraLazy
{
    public class FrameViewer
    {
        frmFrameDumper dumper;

        public FrameViewer()
        {
            this.dumper = new frmFrameDumper();
            List<Frame> frames = InterfaceHelper.GetFrames;
            foreach (Frame frame in frames)
            {
                this.dumper.addFrame(frame.GetName + " " + frame.GetText);
            }
            this.dumper.Show();
        }

        public static void getChildren(string name)
        {
            frmFrameDumper childDumper = new frmFrameDumper();
            try
            {
                List<Frame> frames = InterfaceHelper.GetFrameByName(name).GetChilds;
                foreach (Frame frame in frames)
                {
                    childDumper.addFrame(frame.GetName + " " + frame.GetText);
                }
                childDumper.Show();
            }
            catch
            {
            }
        }
    }
}
