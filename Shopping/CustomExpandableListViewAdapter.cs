using System.Collections.Generic;

using Android.Content;
using Android.Views;
using Android.Widget;

namespace Shopping_List_CSharp
{
    class CustomExpandableListViewAdapter : BaseExpandableListAdapter
    {

        private Context context;
        private List<string> listGroup;
        private Dictionary<string, List<string>> lstChild;

        public CustomExpandableListViewAdapter(Context context, List<string> listGroup, Dictionary<string, List<string>> lstChild)
        {
            this.context = context;
            this.listGroup = listGroup;
            this.lstChild = lstChild;
        }

        public override int GroupCount
        {
            get{ return listGroup.Count; }
        }

        public override bool HasStableIds
        {
            get{ return false; }
        }

        public override Java.Lang.Object GetChild(int groupPosition, int childPosition)
        {
            var result = new List<string>();
            lstChild.TryGetValue(listGroup[groupPosition], out result);
            return result[childPosition];
        }

        public override long GetChildId(int groupPosition, int childPosition)
        {
            return childPosition;
        }

        public override int GetChildrenCount(int groupPosition)
        {
            var result = new List<string>();
            lstChild.TryGetValue(listGroup[groupPosition], out result);
            return result.Count;
        }

        public override View GetChildView(int groupPosition, int childPosition, bool isLastChild, View convertView, ViewGroup parent)
        {
            if (convertView == null)
            {
                LayoutInflater inflater = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService);
                convertView = inflater.Inflate(Resource.Layout.ExpListItem, null);
            }

            TextView textViewItem = convertView.FindViewById<TextView>(Resource.Id.list_item);
            string content = (string)GetChild(groupPosition, childPosition);
            textViewItem.Text = content;
            return convertView;
        }

        public override Java.Lang.Object GetGroup(int groupPosition)
        {
            return listGroup[groupPosition];
        }

        public override long GetGroupId(int groupPosition)
        {
            return groupPosition;
        }

        public override View GetGroupView(int groupPosition, bool isExpanded, View convertView, ViewGroup parent)
        {
            if (convertView == null)
            {
                LayoutInflater inflater = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService);
                convertView = inflater.Inflate(Resource.Layout.ExpListHeader, null);
            }

            string textGroup = (string)GetGroup(groupPosition);
            TextView textViewGroup = convertView.FindViewById<TextView>(Resource.Id.list_header);
            textViewGroup.Text = textGroup;
            return convertView;
        }



        public override bool IsChildSelectable(int groupPosition, int childPosition)
        {
            return true;
        }

    }

}