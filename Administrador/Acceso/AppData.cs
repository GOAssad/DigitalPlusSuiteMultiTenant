using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acceso
{
	public delegate void OnChangeHandler();

	public class AppData
	{
		public const int MaxFingers = 10;
		// shared data
		public int EnrolledFingersMask = 0;
		public int MaxEnrollFingerCount = MaxFingers;
		public bool IsEventHandlerSucceeds = true;
		public bool IsFeatureSetMatched = false;
		public int FalseAcceptRate = 0;
        // public DPFP.Template[] Templates { get; set; } = new DPFP.Template[MaxFingers];
        public DPFP.Template[] Templates = new DPFP.Template[MaxFingers];

        public int MyProperty { get; set; }

        // data change notification
        public void Update() { OnChange(); }        // just fires the OnChange() event
		public event OnChangeHandler OnChange;
	}

    public class TemplateAux
    {
        public Byte[] TByte;
        public string TSize;
        public int TData;
       

    }
}
