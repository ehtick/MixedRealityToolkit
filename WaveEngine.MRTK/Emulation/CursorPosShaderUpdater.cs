﻿// Copyright © Wave Engine S.L. All rights reserved. Use is subject to license terms.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Managers;
using WaveEngine.Mathematics;
using WaveEngine.MRTK.Toolkit.Extensions;

namespace WaveEngine.MRTK.Emulation
{
    /// <summary>
    /// Updates the position of the cursors on all materials with shaders that required it.
    /// </summary>
    public class CursorPosShaderUpdater : UpdatableSceneManager
    {
        private HashSet<Material> materials = new HashSet<Material>();
        private Guid holographicEffectId;

        /// <summary>
        /// Initializes a new instance of the <see cref="CursorPosShaderUpdater"/> class.
        /// </summary>
        /// <param name="holographicEffectId">Id of holographic effect.</param>
        public CursorPosShaderUpdater(Guid holographicEffectId)
        {
            this.holographicEffectId = holographicEffectId;
        }

        /// <inheritdoc/>
        protected override void Start()
        {
            base.Start();

            HashSet<Material> unbatchMaterials = new HashSet<Material>();

            foreach (MaterialComponent m in this.Managers.EntityManager.FindComponentsOfType<MaterialComponent>().ToArray())
            {
                if (m.Material != null && m.Material.Effect.Id == this.holographicEffectId)
                {
                    if (Array.IndexOf(m.Material.ActiveDirectivesNames, "BORDER_LIGHT") != -1 ||
                        Array.IndexOf(m.Material.ActiveDirectivesNames, "INNER_GLOW") != -1)
                    {
                        // Border Light and inner glow don't work if batching is enabled
                        if (unbatchMaterials.Contains(m.Material))
                        {
                            m.Material = m.Material.Clone();
                        }
                        else
                        {
                            unbatchMaterials.Add(m.Material);
                        }
                    }

                    if (Array.IndexOf(m.Material.ActiveDirectivesNames, "NEAR_LIGHT_FADE") != -1 ||
                       Array.IndexOf(m.Material.ActiveDirectivesNames, "HOVER_LIGHT") != -1 ||
                       Array.IndexOf(m.Material.ActiveDirectivesNames, "PROXIMITY_LIGHT") != -1)
                    {
                        // These materials need to be updated with the cursor positions
                        this.materials.Add(m.Material);
                    }
                }
            }
        }

        /// <inheritdoc/>
        public override void Update(TimeSpan gameTime)
        {
            foreach (Material m in this.materials)
            {
                this.UpdateMaterial(m);
            }
        }

        /// <summary>
        /// Update specific material.
        /// </summary>
        /// <param name="mat">The material to update.</param>
        protected void UpdateMaterial(Material mat)
        {
            for (int i = 0; i < HoverLight.activeHoverLights.Count && i < HoverLight.MaxLights; ++i)
            {
                int accessIdx = 320 + (32 * i);

                HoverLight light = HoverLight.activeHoverLights[i];
                mat.CBuffers[1].SetBufferData<Vector3>(light.transform.Position, accessIdx);
                mat.CBuffers[1].SetBufferData<float>(1.0f, accessIdx + 12);
                mat.CBuffers[1].SetBufferData<Vector3>(light.Color.ToVector3(), accessIdx + 16);
                mat.CBuffers[1].SetBufferData<float>(1.0f / MathHelper.Clamp(light.Radius, 0.001f, 1.0f), accessIdx + 28);
            }

            for (int i = 0; i < ProximityLight.MaxLights; ++i)
            {
                int accessIdx = 416 + (96 * i);

                ProximityLight light = i < ProximityLight.activeProximityLights.Count ? ProximityLight.activeProximityLights[i] : null;
                if (light != null)
                {
                    mat.CBuffers[1].SetBufferData<Vector3>(light.transform.Position, accessIdx);
                    mat.CBuffers[1].SetBufferData<float>(1.0f, accessIdx + 12);

                    float pulseScaler = 1.0f; // + light.pulseTime;
                    Vector4 v4 = new Vector4(
                            light.NearRadius * pulseScaler,
                            1.0f / MathHelper.Clamp(light.FarRadius * pulseScaler, 0.001f, 1.0f),
                            1.0f / MathHelper.Clamp(light.NearDistance * pulseScaler, 0.001f, 1.0f),
                            MathHelper.Clamp(light.MinNearSizePercentage, 0.0f, 1.0f));
                    mat.CBuffers[1].SetBufferData<Vector4>(
                        v4,
                        accessIdx + 16);
                    v4 = new Vector4(
                            light.NearDistance * light.pulseTime,
                            MathHelper.Clamp(1.0f - light.pulseFade, 0.0f, 1.0f),
                            0.0f,
                            0.0f);
                    mat.CBuffers[1].SetBufferData<Vector4>(
                        v4,
                        accessIdx + 32);
                    mat.CBuffers[1].SetBufferData<Vector4>(light.CenterColor.ToVector4(), accessIdx + 48);
                    mat.CBuffers[1].SetBufferData<Vector4>(light.MiddleColor.ToVector4(), accessIdx + 64);
                    mat.CBuffers[1].SetBufferData<Vector4>(light.OuterColor.ToVector4(), accessIdx + 80);
                }
                else
                {
                    mat.CBuffers[1].SetBufferData<Vector4>(Vector4.Zero, accessIdx);
                }
            }
        }
    }
}