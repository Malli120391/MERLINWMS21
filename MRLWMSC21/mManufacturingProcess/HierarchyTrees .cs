using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRLWMSC21.mManufacturingProcess
{
    public class HierarchyTrees : List <HierarchyTrees.HTree>
    {

        public class HTree
        {
            private string m_NodeDescription;
            private string m_MDescription;
            private int m_UnderParent;
            private int m_LevelDepth;
            private int m_NodeID;


            public string MDescription
            {
                get { return m_MDescription; }
                set { m_MDescription = value; }
            }

            public int NodeID
            {
                get { return m_NodeID; }
                set { m_NodeID = value; }
            }

            public string NodeDescription
            {
                get { return m_NodeDescription; }
                set { m_NodeDescription = value; }
            }
            public int UnderParent
            {
                get { return m_UnderParent; }
                set { m_UnderParent = value; }
            }
            public int LevelDepth
            {
                get { return m_LevelDepth; }
                set { m_LevelDepth = value; }
            }
        } 

    }
}