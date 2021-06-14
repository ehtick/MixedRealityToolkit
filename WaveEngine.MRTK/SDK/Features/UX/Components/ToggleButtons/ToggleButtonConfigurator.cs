﻿// Copyright © Wave Engine S.L. All rights reserved. Use is subject to license terms.

using WaveEngine.Framework;
using WaveEngine.MRTK.SDK.Features.UX.Components.Configurators;
using WaveEngine.MRTK.SDK.Features.UX.Components.States;

namespace WaveEngine.MRTK.SDK.Features.UX.Components.ToggleButtons
{
    /// <summary>
    /// Button configuration for toggle states.
    /// </summary>
    [AllowMultipleInstances]
    public class ToggleButtonConfigurator : StandardButtonConfigurator, IStateAware<ToggleState>
    {
        /// <inheritdoc />
        public ToggleState TargetState { get; set; }
    }
}
