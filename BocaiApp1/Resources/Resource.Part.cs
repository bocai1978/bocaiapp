using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
namespace Com.Bocaihua.APP
{
	public partial class Resource 
    {
		public partial class String
		{
#if DEBUG
			public const int package_name = package_name_debug;
#else
			public const int package_name = package_name_release;
#endif
		}
	}
}