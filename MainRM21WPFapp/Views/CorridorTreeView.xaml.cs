using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MainRM21WPFapp.ViewModels;

namespace MainRM21WPFapp.Views
{
   /// <summary>
   /// Interaction logic for CorridorTreeView.xaml
   /// </summary>
   public partial class CorridorTreeView : UserControl
   {
      public CorridorTreeView()
      {
         InitializeComponent();
      }

      private CorridorTreeViewModel corrTVM_;
      public CorridorTreeViewModel aCorridorTreeViewModel
      {
         get { return corrTVM_; }
         set
         {
            corrTVM_ = value;
            base.DataContext = corrTVM_;
         }
      }  //Next: attempt to create the actual user control
   }// then try to get it to work on a window
}
