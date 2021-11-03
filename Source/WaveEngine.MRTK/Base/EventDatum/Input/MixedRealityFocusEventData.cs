﻿// Copyright © Wave Engine S.L. All rights reserved. Use is subject to license terms.

using WaveEngine.Framework;
using WaveEngine.MRTK.Emulation;

namespace WaveEngine.MRTK.Base.EventDatum.Input
{
    /// <summary>
    /// Focus event data.
    /// </summary>
    public class MixedRealityFocusEventData
    {
        /// <summary>
        /// Gets or sets the cursor component.
        /// </summary>
        public Cursor Cursor { get; set; }

        /// <summary>
        /// Gets or sets the current target.
        /// </summary>
        public Entity CurrentTarget { get; set; }
    }
}