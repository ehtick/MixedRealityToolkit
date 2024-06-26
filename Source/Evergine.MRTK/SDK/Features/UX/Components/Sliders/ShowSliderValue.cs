﻿// Copyright © Evergine S.L. All rights reserved. Use is subject to license terms.

using Evergine.Components.Fonts;
using Evergine.Framework;

namespace Evergine.MRTK.SDK.Features.UX.Components.Sliders
{
    /// <summary>
    /// A component to show a parent <see cref="PinchSlider"/> value.
    /// </summary>
    public class ShowSliderValue : Component
    {
        [BindComponent]
        private Text3DMesh text3D = null;

        [BindComponent(source: BindComponentSource.Parents)]
        private PinchSlider pinchSlider = null;

        /// <inheritdoc/>
        protected override void OnActivated()
        {
            base.OnActivated();
            this.pinchSlider.ValueUpdated += this.PinchSlider_ValueUpdated;

            this.UpdateText(this.pinchSlider.SliderValue);
        }

        /// <inheritdoc/>
        protected override void OnDeactivated()
        {
            base.OnDeactivated();
            this.pinchSlider.ValueUpdated -= this.PinchSlider_ValueUpdated;
        }

        private void PinchSlider_ValueUpdated(object sender, SliderEventData e)
        {
            this.UpdateText(e.NewValue);
        }

        private void UpdateText(float value)
        {
            this.text3D.Text = $"{value:F2}";
        }
    }
}
