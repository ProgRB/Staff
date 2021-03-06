﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Windows.Media;

namespace Staff
{
    public static class DataGridHelper
    {

        /// <summary>
        /// Finds a parent of a given item on the visual tree.
        /// </summary>
        /// <typeparam name="T">The type of the queried item.</typeparam>
        /// <param name="child">A direct or indirect child of the
        /// queried item.</param>
        /// <returns>The first parent item that matches the submitted
        /// type parameter. If not matching item can be found, a null
        /// reference is being returned.</returns>
        public static T TryFindParent<T>(this DependencyObject child)
            where T : DependencyObject
        {
            //get parent item
            DependencyObject parentObject = GetParentObject(child);

            //we've reached the end of the tree
            if (parentObject == null) return null;

            //check if the parent matches the type we're looking for
            T parent = parentObject as T;
            if (parent != null)
            {
                return parent;
            }
            else
            {
                //use recursion to proceed with next level
                return TryFindParent<T>(parentObject);
            }
        }

        /// <summary>
        /// This method is an alternative to WPF's
        /// <see cref="VisualTreeHelper.GetParent"/> method, which also
        /// supports content elements. Keep in mind that for content element,
        /// this method falls back to the logical tree of the element!
        /// </summary>
        /// <param name="child">The item to be processed.</param>
        /// <returns>The submitted item's parent, if available. Otherwise
        /// null.</returns>
        public static DependencyObject GetParentObject(this DependencyObject child)
        {
            if (child == null) return null;

            //handle content elements separately
            ContentElement contentElement = child as ContentElement;
            if (contentElement != null)
            {
                DependencyObject parent = ContentOperations.GetParent(contentElement);
                if (parent != null) return parent;

                FrameworkContentElement fce = contentElement as FrameworkContentElement;
                return fce != null ? fce.Parent : null;
            }

            //also try searching for parent in framework elements (such as DockPanel, etc)
            FrameworkElement frameworkElement = child as FrameworkElement;
            if (frameworkElement != null)
            {
                DependencyObject parent = frameworkElement.Parent;
                if (parent != null) return parent;
            }

            //if it's not a ContentElement/FrameworkElement, rely on VisualTreeHelper
            return VisualTreeHelper.GetParent(child);
        }


        /// <summary>
        /// Tries to locate a given item within the visual tree,
        /// starting with the dependency object at a given position. 
        /// </summary>
        /// <typeparam name="T">The type of the element to be found
        /// on the visual tree of the element at the given location.</typeparam>
        /// <param name="reference">The main element which is used to perform
        /// hit testing.</param>
        /// <param name="point">The position to be evaluated on the origin.</param>
        public static T TryFindFromPoint<T>(UIElement reference, System.Windows.Point point)
          where T : DependencyObject
        {
            DependencyObject element = reference.InputHitTest(point)
                                         as DependencyObject;
            if (element == null) return null;
            else if (element is T) return (T)element;
            else return TryFindParent<T>(element);
        }



    }

    public class DataGridAddition
    {
        public static readonly DependencyProperty DoubleClickCommandProperty =
                DependencyProperty.RegisterAttached("DoubleClickCommand", typeof(RoutedUICommand), typeof(DataGridAddition),
                    new PropertyMetadata(OnDoubleClick_PropertyChanged)),
                DoubleClickParameterProperty =
                DependencyProperty.RegisterAttached("DoubleClickParameter", typeof(object), typeof(DataGridAddition));
        public static RoutedUICommand GetDoubleClickCommand(DependencyObject e)
        {
            return (RoutedUICommand)e.GetValue(DoubleClickCommandProperty);
        }
        public static void SetDoubleClickCommand(DependencyObject obj, RoutedUICommand e)
        {
            obj.SetValue(DoubleClickCommandProperty, e);
        }
        public static object GetDoubleClickParameter(DependencyObject e)
        {
            return e.GetValue(DoubleClickParameterProperty);
        }
        public static void SetDoubleClickParameter(DependencyObject obj, RoutedUICommand e)
        {
            obj.SetValue(DoubleClickParameterProperty, e);
        }
        public static void OnDoubleClick_PropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            SetDataGridDoubleClick(sender as Control, e);
        }
        private static void SetDataGridDoubleClick(Control sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != e.OldValue)
                if (e.NewValue != DataGridAddition.DoubleClickCommandProperty.DefaultMetadata.DefaultValue)
                    sender.MouseDoubleClick += new MouseButtonEventHandler(sender_MouseDoubleClick);
                else
                    sender.MouseDoubleClick -= new MouseButtonEventHandler(sender_MouseDoubleClick);
        }

        static void sender_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            IInputElement elem = e.MouseDevice.DirectlyOver;
            if (e.ChangedButton == MouseButton.Left && elem != null && elem is FrameworkElement)
            {
                FrameworkElement f = elem as FrameworkElement;
                if (f.TryFindParent<DataGridRow>() != null || f.TryFindParent<Xceed.Wpf.DataGrid.Cell>() != null)
                {
                    Control dg = sender as Control;
                    RoutedUICommand r = dg.GetValue(DataGridAddition.DoubleClickCommandProperty) as RoutedUICommand;
                    object parameter = dg.GetValue(DataGridAddition.DoubleClickParameterProperty);
                    if (r != null)
                        r.Execute(parameter, elem);
                }
            }
        }
    }
}
