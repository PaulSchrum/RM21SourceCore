﻿using System;
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

namespace MainRM21WPFapp
{
   /// <summary>
   /// Interaction logic for MainWindow.xaml
   /// </summary>
   public partial class MainWindow : Window
   {
      private MainWindowVM topVM { get; set; }
      public MainWindow()
      {
         InitializeComponent();
         topVM = (MainWindowVM) DataContext;
         topVM.myViewReference = this;
         btn_advance.Focus();
      }

      private void xsCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
      {
         Canvas senderCanvas = (Canvas)sender;
         if (!(senderCanvas.DataContext is RoadwayModel_TabVM)) return;

         RoadwayModel_TabVM rvm = (RoadwayModel_TabVM)senderCanvas.DataContext;
         rvm.CrossSectionViewModel.MouseLeftButtonDown(senderCanvas, e);
      }

      private void xsCanvas_MouseMove(object sender, MouseEventArgs e)
      {
         Canvas senderCanvas = (Canvas)sender;
         if (!(senderCanvas.DataContext is RoadwayModel_TabVM)) return;

         RoadwayModel_TabVM rvm = (RoadwayModel_TabVM)senderCanvas.DataContext;
         rvm.CrossSectionViewModel.MouseMove(senderCanvas, e);
      }

      private void xsCanvas_MouseLeftButtonUp(object sender, MouseEventArgs e)
      {
         Canvas senderCanvas = (Canvas)sender;
         if (!(senderCanvas.DataContext is RoadwayModel_TabVM)) return;

         RoadwayModel_TabVM rvm = (RoadwayModel_TabVM)senderCanvas.DataContext;
         rvm.CrossSectionViewModel.MouseLeftButtonUp(senderCanvas, e);
      }

      private void xsCanvas_MouseWheel(object sender, MouseWheelEventArgs e)
      {
         Canvas senderCanvas = (Canvas)sender;
         if (!(senderCanvas.DataContext is RoadwayModel_TabVM)) return;

         RoadwayModel_TabVM rvm = (RoadwayModel_TabVM)senderCanvas.DataContext;
         rvm.CrossSectionViewModel.MouseWheel(senderCanvas, e);
      }

      private void TextBox_MouseWheel(object sender, MouseWheelEventArgs e)
      {
         Object obj = ((TextBox)sender).DataContext;
         MainWindowVM topVM = (MainWindowVM)DataContext;
         topVM.myViewReference = this;
         RoadwayModel_TabVM rvm = topVM.RoadwayModelTabVM;
         rvm.CrossSectionViewModel.StationTextMouseWheel(sender, e);
      }

      private void Window_ContentRendered(object sender, EventArgs e)
      {
         topVM.updateCanvases();
      }

      private void selectAllText(TextBox aTextBox)
      {
         aTextBox.SelectAll();
      }

      private void TextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
      {
         if (!(sender is TextBox)) return;

         TextBox thisTextBox = (TextBox)sender;
         selectAllText(thisTextBox);
      }

      private void psCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
      {
         Canvas senderCanvas = (Canvas)sender;
         if (!(senderCanvas.DataContext is RoadwayModel_TabVM)) return;

         RoadwayModel_TabVM rvm = (RoadwayModel_TabVM)senderCanvas.DataContext;
         rvm.PlanSchematicViewModel.MouseLeftButtonDown(senderCanvas, e);
      }

      private void psCanvas_MouseMove(object sender, MouseEventArgs e)
      {
         Canvas senderCanvas = (Canvas)sender;
         if (!(senderCanvas.DataContext is RoadwayModel_TabVM)) return;

         RoadwayModel_TabVM rvm = (RoadwayModel_TabVM)senderCanvas.DataContext;
         rvm.PlanSchematicViewModel.MouseMove(senderCanvas, e);
      }

      private void psCanvas_MouseLeftButtonUp(object sender, MouseEventArgs e)
      {
         Canvas senderCanvas = (Canvas)sender;
         if (!(senderCanvas.DataContext is RoadwayModel_TabVM)) return;

         RoadwayModel_TabVM rvm = (RoadwayModel_TabVM)senderCanvas.DataContext;
         rvm.PlanSchematicViewModel.MouseLeftButtonUp(senderCanvas, e);
      }

      private void psCanvas_MouseWheel(object sender, MouseWheelEventArgs e)
      {
         Canvas senderCanvas = (Canvas)sender;
         if (!(senderCanvas.DataContext is RoadwayModel_TabVM)) return;

         RoadwayModel_TabVM rvm = (RoadwayModel_TabVM)senderCanvas.DataContext;
         rvm.PlanSchematicViewModel.MouseWheel(senderCanvas, e);
      }

   }
}
