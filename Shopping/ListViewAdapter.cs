using Android.App;
using Android.Graphics;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Shopping_List_CSharp
{
    class ListViewAdapter : BaseAdapter<ShoppingList>
    {

        ShoppingList[] shoppingLists;
        Activity context;

        public ListViewAdapter(Activity context, ShoppingList[] shoppingLists) : base()
        {
            this.context = context;
            this.shoppingLists = shoppingLists;
        }


        public override long GetItemId(int position)
        {
            return position;
        }


        public override ShoppingList this[int position]
        {
            get { return shoppingLists[position]; }
        }


        public override int Count
        {
            get { return shoppingLists.Length; }
        }


        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView; 
            if (view == null) 
                view = context.LayoutInflater.Inflate(Resource.Layout.ListViewItem, null);

            Log.Debug("Liste", "Nom : " + shoppingLists[position].Nom + " Couleur : " + shoppingLists[position].Red + " " + shoppingLists[position].Green + " " + shoppingLists[position].Blue);

            TextView content = view.FindViewById<TextView>(Resource.Id.shoppingListName);
            content.Text = shoppingLists[position].Nom;
            content.SetTextColor(Color.Argb(255, shoppingLists[position].Red, shoppingLists[position].Green, shoppingLists[position].Blue));

            return view;
        }

    }

}