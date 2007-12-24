#region Copyright (C) 2007 Team MediaPortal

/*
    Copyright (C) 2007 Team MediaPortal
    http://www.team-mediaportal.com
 
    This file is part of MediaPortal II

    MediaPortal II is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    MediaPortal II is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with MediaPortal II.  If not, see <http://www.gnu.org/licenses/>.
*/
#endregion

using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using MediaPortal.Core.Properties;

namespace SkinEngine.Controls.Visuals
{
  public class UIElement : Visual
  {
    Property _visibleProperty;
    protected Size _desiredSize;
    Property _acutalPositionProperty;
    Property _positionProperty;
    Property _dockProperty;
    bool _layoutValid;

    public UIElement()
    {
      _visibleProperty = new Property((bool)true);
      _acutalPositionProperty = new Property(new Vector3(0, 0, 1));
      _positionProperty = new Property(new Vector3(0, 0, 1));
      _dockProperty = new Property(Dock.Top);

      _visibleProperty.Attach(new PropertyChangedHandler(OnPropertyChanged));
      _positionProperty.Attach(new PropertyChangedHandler(OnPropertyChanged));
      _dockProperty.Attach(new PropertyChangedHandler(OnPropertyChanged));

    }

    void OnPropertyChanged(Property property)
    {
      Invalidate();
    }

    public Property ActualPositionProperty
    {
      get
      {
        return _acutalPositionProperty;
      }
      set
      {
        _acutalPositionProperty = value;
      }
    }

    public Vector3 ActualPosition
    {
      get
      {
        return (Vector3)_acutalPositionProperty.GetValue();
      }
      set
      {
        _acutalPositionProperty.SetValue(value);
      }
    }

    public Property PositionProperty
    {
      get
      {
        return _positionProperty;
      }
      set
      {
        _positionProperty = value;
      }
    }

    public Vector3 Position
    {
      get
      {
        return (Vector3)_positionProperty.GetValue();
      }
      set
      {
        _positionProperty.SetValue(value);
      }
    }
    public Property DockProperty
    {
      get
      {
        return _dockProperty;
      }
      set
      {
        _dockProperty = value;
      }
    }

    public Dock Dock
    {
      get
      {
        return (Dock)_dockProperty.GetValue();
      }
      set
      {
        _dockProperty.SetValue(value);
      }
    }
    /// <summary>
    /// Gets or sets the is visible property.
    /// </summary>
    /// <value>The is visible property.</value>
    public Property IsVisibleProperty
    {
      get
      {
        return _visibleProperty;
      }
      set
      {
        _visibleProperty = value;
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether this instance is visible.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if this instance is visible; otherwise, <c>false</c>.
    /// </value>
    public bool IsVisible
    {
      get
      {
        return (bool)_visibleProperty.GetValue();
      }
      set
      {
        _visibleProperty.SetValue(value);
      }
    }

    public bool LayoutValid
    {
      get
      {
        return _layoutValid;
      }
      set
      {
        _layoutValid = value;
      }
    }

    public Size DesiredSize
    {
      get
      {
        return _desiredSize;
      }
    }
    /// <summary>
    /// Gets the size for brush.
    /// </summary>
    /// <param name="width">The width.</param>
    /// <param name="height">The height.</param>
    public virtual void GetSizeForBrush(out double width, out double height)
    {
      width = 0.0;
      height = 0.0;
    }

    /// <summary>
    /// measures the size in layout required for child elements and determines a size for the FrameworkElement-derived class.
    /// </summary>
    /// <param name="availableSize">The available size that this element can give to child elements. </param>
    /// <returns>The size that this element determines it needs during layout, based on its calculations of child element sizes.</returns>
    public virtual void Measure(Size availableSize)
    {
      _desiredSize = new Size(0, 0);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="finalRect">The final size that the parent computes for the child element</param>
    public virtual void Arrange(Rectangle finalRect)
    {
      LayoutValid = true;
    }

    public virtual void Invalidate()
    {
      if (!LayoutValid) return;
      System.Drawing.Size sizeOld = _desiredSize;
      Measure(sizeOld);
      if (_desiredSize == sizeOld)
      {
        Arrange(new Rectangle((int)ActualPosition.X, (int)ActualPosition.Y, _desiredSize.Width, _desiredSize.Height));
      }
      else if (VisualParent != null)
      {
        VisualParent.Invalidate();
      }
      else
      {
        Measure(new Size((int)SkinContext.Width, (int)SkinContext.Height));
        Arrange(new Rectangle(0, 0, (int)SkinContext.Width, (int)SkinContext.Height));
      }
    }
  }
}
