using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using rm21Core;
using ptsCogo;
using System.Collections.ObjectModel;

namespace MainRM21WPFapp.ViewModels
{
   public class RibbonViewModel : TreeViewItemViewModel
   {
      public RibbonViewModel(TreeViewItemViewModel parent) : base(parent)
      { }

      public RibbonViewModel(ribbonBase newRibbon)
         : base(null)
      {
         theRibbon_ = newRibbon;

         if (theRibbon_.Widths.SegmentCount > 0)
            WidthsVM = new ProfileVPI_VM(theRibbon_.Widths);

         if (theRibbon_.CrossSlopes.SegmentCount > 0)
            CrossSlopesVM = new ProfileVPI_VM(theRibbon_.CrossSlopes);
      }

      private ribbonBase theRibbon_;
      internal ribbonBase TheRibbon
      { get { return theRibbon_; } }

      public new String HashName
      {
         get { return theRibbon_.getHashName(); }
      }

      private ProfileVPI_VM widthsVM_;
      public ProfileVPI_VM WidthsVM
      {
         get { return widthsVM_; }
         set
         {
            if (widthsVM_ != value)
            {
               widthsVM_ = value;

               RaisePropertyChanged("WidthsVM");
            }
         }
      }

      private ProfileVPI_VM crossSlopes_;
      public ProfileVPI_VM CrossSlopesVM
      {
         get { return crossSlopes_; }
         set
         {
            if (crossSlopes_ != value)
            {
               crossSlopes_ = value;

               RaisePropertyChanged("CrossSlopes");
            }
         }
      }
   }

   public class ProfileVPI_VM : TreeViewItemViewModel
   {
      public ProfileVPI_VM(Profile aProfile) : base(null)
      {
         theProfile_ = aProfile;
         rawVPI_VM = new ObservableCollection<RawVPI_VM>();
         foreach (var aRawVPI in theProfile_.VpiList.theVPIs)
         {
            rawVPI_VM.Add(new RawVPI_VM(aRawVPI));
         }
      }

      private Profile theProfile_;

      public ObservableCollection<RawVPI_VM> rawVPI_VM { get; set; }
   }

   public class RawVPI_VM : TreeViewItemViewModel
   {
      public RawVPI_VM(rawVPI aRawVPI)
         : base(null)
      {
         StationVM = new Station_VM(aRawVPI.Station);
         ElevationVM = new Elevation_VM(aRawVPI.Elevation);
      }

      private Station_VM stationVM_;
      public Station_VM StationVM
      {
         get { return stationVM_; }
         set
         {
            if (stationVM_ != value)
            {
               stationVM_ = value;
               RaisePropertyChanged("StationVM");

            }
         }
      }

      private Elevation_VM elevationVM_;
      public Elevation_VM ElevationVM
      {
         get { return elevationVM_; }
         set
         {
            if (elevationVM_ != value)
            {
               elevationVM_ = value;
               RaisePropertyChanged("ElevationVM");

            }
         }
      }
   }

   public class Station_VM : NotifyPropertyChangedVMbase
   {
      public Station_VM(CogoStation aStation)
      {
         theStation_ = aStation;
      }

      private CogoStation theStation_;
      public double Val
      {
         get { return theStation_.trueStation; }
         set
         {
            if (theStation_.trueStation != value)
            {
               theStation_.trueStation = value;
               RaisePropertyChanged("Val");

            }
         }
      }

   }

   public class Elevation_VM : NotifyPropertyChangedVMbase
   {
      public Elevation_VM(double anElevation)
      {
         theElevation_ = anElevation;
      }

      private double theElevation_;
      public double Val
      {
         get { return theElevation_; }
         set
         {
            if (theElevation_ != value)
            {
               theElevation_ = value;
               RaisePropertyChanged("Val");

            }
         }
      }
   }
}
