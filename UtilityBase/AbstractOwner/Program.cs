using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractOwner
{
    class Program
    {
        static void Main(string[] args)
        {
            DocMaker1_1 doc1_1 = new DocMaker1_1();
            doc1_1.CreateDocument();

            DocMaker1_9 doc1_9 = new DocMaker1_9();
            doc1_9.CreateDocument();

            Console.ReadKey();
        }
    }

    public abstract class DocMaker
    {
        public void CreateDocument()
        {
            string tmp = string.Empty;

            tmp += this.SetDataKey() + Environment.NewLine;

            tmp += this.ScheduleMaker() + Environment.NewLine;

            tmp += this.Settle_Wording1() + Environment.NewLine;

            Console.WriteLine(tmp);
        }

        public abstract string SetDataKey();

        public abstract string ScheduleMaker();

        public virtual string Settle_Wording1()
        {
            return "yeah~";
        }
    }

    public class DocMaker1_1 : DocMaker
    {
        public override string SetDataKey()
        {
            return "DocMaker1_1 SetDataKey";
        }

        public override string ScheduleMaker()
        {
            return "DocMaker1_1 ScheduleMaker";
        }
    }

    public class DocMaker1_9 : DocMaker
    {
        public override string SetDataKey()
        {
            return "DocMaker1_9 SetDataKey";
        }

        public override string ScheduleMaker()
        {
            return "DocMaker1_9 ScheduleMaker";
        }

        public override string Settle_Wording1()
        {
            return "DocMaker1_9 yeah~";
        }
    }
}
