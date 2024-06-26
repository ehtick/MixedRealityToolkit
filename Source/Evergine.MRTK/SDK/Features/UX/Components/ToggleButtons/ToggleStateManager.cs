﻿// Copyright © Evergine S.L. All rights reserved. Use is subject to license terms.

using System;
using System.Collections.Generic;
using System.Linq;
using Evergine.Common.Attributes;
using Evergine.Framework;
using Evergine.MRTK.SDK.Features.UX.Components.States;

namespace Evergine.MRTK.SDK.Features.UX.Components.ToggleButtons
{
    /// <summary>
    /// State component for toggle.
    /// </summary>
    public class ToggleStateManager : BaseStateManager<ToggleState>
    {
        [BindComponent(source: BindComponentSource.Parents, isRequired: false)]
        private ToggleGroup toggleGroup = null;

        /// <summary>
        /// Gets or sets a value indicating whether default components should be added.
        /// </summary>
        [DontRenderProperty]
        public bool DefaultComponentsAdded { get; set; }

        /// <inheritdoc />
        public override void ChangeState(State<ToggleState> newState)
        {
            base.ChangeState(newState);

            if (this.toggleGroup != null && this.CurrentState.Value == ToggleState.On)
            {
                var entityManager = this.Managers.EntityManager;
                var allToggleGroups = entityManager
                    .FindComponentsOfType<ToggleGroup>(isExactType: false)
                    .Where(group => group.Name == this.toggleGroup.Name)
                    .ToArray();

                for (int i = 0; i < allToggleGroups.Length; i++)
                {
                    var currentGroup = allToggleGroups[i];
                    var managers = currentGroup.Owner
                        .FindComponentsInChildren<ToggleStateManager>(isExactType: false)
                        .ToArray();

                    for (int j = 0; j < managers.Length; j++)
                    {
                        var manager = managers[j];
                        if (manager != this)
                        {
                            var offState = manager.States.FirstOrDefault(state => state.Value == ToggleState.Off);
                            manager.ChangeState(offState);
                        }
                    }
                }
            }
        }

        /// <inheritdoc />
        protected override bool OnAttached()
        {
            if (!base.OnAttached())
            {
                return false;
            }

            this.AddDefaultComponents();

            return true;
        }

        /// <summary>
        /// State for when the toggle is on.
        /// </summary>
        public static readonly State<ToggleState> State_On = new State<ToggleState>() { Name = ToggleState.On.ToString(), Value = ToggleState.On };

        /// <summary>
        /// State for when the toggle is off.
        /// </summary>
        public static readonly State<ToggleState> State_Off = new State<ToggleState>() { Name = ToggleState.Off.ToString(), Value = ToggleState.Off };

        /// <inheritdoc />
        protected override List<State<ToggleState>> GetStateList()
        {
            var states = new List<State<ToggleState>>();
            states.Add(State_Off);
            states.Add(State_On);
            return states;
        }

        /// <inheritdoc />
        protected override bool CanChangeState()
        {
            if (this.toggleGroup == null || this.toggleGroup.AllowOff)
            {
                return true;
            }

            return this.CurrentState.Value == ToggleState.Off;
        }

        private void AddDefaultComponents()
        {
            if (this.DefaultComponentsAdded)
            {
                return;
            }

            var allConfigurations = this.Owner.FindComponents<ToggleButtonConfigurator>(isExactType: false);
            var allStates = Enum.GetValues(typeof(ToggleState))
                .Cast<ToggleState>()
                .ToArray();

            for (int i = 0; i < allStates.Length; i++)
            {
                var state = allStates[i];
                if (!allConfigurations.Any(config => config.TargetState == state))
                {
                    this.Owner.AddComponent(new ToggleButtonConfigurator() { TargetState = state });
                }
            }

            this.DefaultComponentsAdded = true;
        }
    }
}
