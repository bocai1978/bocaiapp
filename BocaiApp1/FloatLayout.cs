using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Com.Bocaihua.APP
{
    public class FloatLayout : RelativeLayout, Android.Views.View.IOnDragListener
    {
        public FloatLayout(Context context): base(context)
        {
            
        }
        public int draggedIndex = 0;
        public int droppedIndex = 0;
        public bool OnDrag(View v, DragEvent e)
        {
            switch (e.Action)
            {
                case DragAction.Started:

                    return true;
                case DragAction.Entered:

                    return true;
                case DragAction.Exited:

                    return true;
                case DragAction.Drop:
                    
                    droppedIndex = Convert.ToInt16(this.GetTag(Resource.String.keyval)); ;
                    return true;
                case DragAction.Ended:

                    return true;

            }
            return false;

        }
    }
}