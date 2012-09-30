using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MainRM21WPFapp.ViewModels
{
   /// <summary>
   /// Base class for all ViewModel classes displayed by TreeViewItems.  
   /// This acts as an adapter between a raw data object and a TreeViewItem.
   /// Copied from Josh Smith:
   /// http://www.codeproject.com/Articles/26288/Simplifying-the-WPF-TreeView-by-Using-the-ViewMode?fid=1342639&select=2941318&fr=1#xx0xx
   /// </summary>
   public class TreeViewItemViewModel : NotifyPropertyChangedVMbase
   {
      #region Data

      static readonly TreeViewItemViewModel DummyChild = new TreeViewItemViewModel();

      readonly ObservableCollection<TreeViewItemViewModel> _children;

      bool _isExpanded;
      bool _isSelected;

      #endregion // Data

      #region Constructors

      protected TreeViewItemViewModel(TreeViewItemViewModel parent)
      {
         Parent = parent;

         _children = new ObservableCollection<TreeViewItemViewModel>();
         IsExpanded = true;
      }

      // This is used to create the DummyChild instance.
      private TreeViewItemViewModel()
      {
      }

      #endregion // Constructors

      #region Presentation Members
      #region Children

      /// <summary>
      /// Returns the logical child items of this object.
      /// </summary>
      public ObservableCollection<TreeViewItemViewModel> Children
      {
         get { return _children; }
      }

      #endregion // Children

      #region HasLoadedChildren

      /// <summary>
      /// Returns true if this object's Children have not yet been populated.
      /// </summary>
      public bool HasDummyChild
      {
         get { return this.Children.Count == 1 && this.Children[0] == DummyChild; }
      }

      #endregion // HasLoadedChildren

      #region IsExpanded

      /// <summary>
      /// Gets/sets whether the TreeViewItem 
      /// associated with this object is expanded.
      /// </summary>
      public bool IsExpanded
      {
         get { return _isExpanded; }
         set
         {
            if (value != _isExpanded)
            {
               _isExpanded = value;
               this.OnPropertyChanged("IsExpanded");
            }

            // Expand all the way up to the root.
            if (_isExpanded && _parent != null)
               _parent.IsExpanded = true;


            //this.LoadChildren();
         }
      }

      #endregion // IsExpanded

      #region IsSelected

      /// <summary>
      /// Gets/sets whether the TreeViewItem 
      /// associated with this object is selected.
      /// </summary>
      public bool IsSelected
      {
         get { return _isSelected; }
         set
         {
            if (value != _isSelected)
            {
               _isSelected = value;
               this.OnPropertyChanged("IsSelected");
               if (this is RibbonViewModel)
               {
                  (this as RibbonViewModel).OnIsSelected();
               }
            }
         }
      }

      #endregion // IsSelected

      #region HashName
      private String hashName_;
      public String HashName
      {
         get { return hashName_; }
         protected set
         {
            if (value != hashName_)
            {
               hashName_ = value;
               this.OnPropertyChanged("HashName");
            }
         }
      }
      #endregion // HashName  

      #region LoadChildren

      /// <summary>
      /// Invoked when the child items need to be loaded on demand.
      /// Subclasses can override this to populate the Children collection.
      /// </summary>
      //protected virtual void LoadChildren()
      //{
      //}

      #endregion // LoadChildren

      #region Parent
      private TreeViewItemViewModel _parent;
      public TreeViewItemViewModel Parent
      {
         get { return _parent; }
         internal set 
         { 
            _parent = value;
            if (null != this.Children)
               foreach (var child in Children)
                  child.Parent = this;
         }
      }
      #endregion // Parent

      #endregion // Presentation Members

      public RoadwayModel_TabVM getRoadwayModelVM()
      {

         TreeViewItemViewModel topParent = this;
         while (topParent.Parent != null)
         {
            topParent = topParent.Parent;
         }

         if (!(topParent is CorridorTreeViewModel))
            return null;  // this represents a logic failure

         CorridorTreeViewModel corridorTVM = topParent as CorridorTreeViewModel;
         return corridorTVM.ownerRoadwayVM;
      }

   }
}