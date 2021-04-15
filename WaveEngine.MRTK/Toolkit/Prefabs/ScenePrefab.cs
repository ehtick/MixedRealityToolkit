﻿// Copyright © Wave Engine S.L. All rights reserved. Use is subject to license terms.

using System;
using System.Diagnostics;
using System.Linq;
using WaveEngine.Common.Attributes;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework;
using WaveEngine.Framework.Assets;
using WaveEngine.Framework.Services;

namespace WaveEngine.MRTK.Toolkit.Prefabs
{
    /// <summary>
    /// A component that can be used to instantiate prefabs using scenes.
    /// </summary>
    public class ScenePrefab : Component
    {
        /// <summary>
        /// Gets or sets the asset service.
        /// </summary>
        [BindService]
        public AssetsService AssetsService;

        private bool duplicateMaterials;

        /// <summary>
        /// Gets the scene prefab ID that will be used.
        /// </summary>
        [RenderProperty(CustomPropertyName = "Prefab")]
        public ScenePrefabProperty ScenePrefabProperty = new ScenePrefabProperty();

        /// <summary>
        /// Gets or sets a value indicating whether the materials that this prefab uses will be duplicated.
        /// </summary>
        [RenderProperty(Tooltip = "Set whether the materials that this prefab uses will be duplicated.")]
        public bool DuplicateMaterials
        {
            get => this.duplicateMaterials;
            set
            {
                if (this.duplicateMaterials != value)
                {
                    this.duplicateMaterials = value;
                    this.RefreshEntity();
                }
            }
        }

        /// <inheritdoc/>
        protected override bool OnAttached()
        {
            if (!base.OnAttached())
            {
                return false;
            }

            this.ScenePrefabProperty.OnScenePrefabChanged += this.ScenePrefab_OnScenePrefabChanged;

            this.RefreshEntity(false);

            return true;
        }

        /// <inheritdoc/>
        protected override void OnDetach()
        {
            this.ClearEntity();

            this.ScenePrefabProperty.OnScenePrefabChanged -= this.ScenePrefab_OnScenePrefabChanged;

            base.OnDetach();
        }

        private void ScenePrefab_OnScenePrefabChanged(object sender, EventArgs e)
        {
            this.RefreshEntity();
        }

        private void ClearEntity()
        {
            while (this.Owner.NumChildren > 0)
            {
                this.Owner.RemoveChild(this.Owner.ChildEntities.First());
            }
        }

        /// <summary>
        /// Refresh the entity, reinstancing the prefab.
        /// </summary>
        /// <param name="checkIsAttached">Check if it's attached.</param>
        public void RefreshEntity(bool checkIsAttached = true)
        {
            if (checkIsAttached &&
                !this.IsAttached)
            {
                return;
            }

            this.ClearEntity();

            if (!this.ScenePrefabProperty.IsPrefabIdValid)
            {
                return;
            }

            var st = Stopwatch.StartNew();
            var source = this.AssetsService.GetAssetSource<SceneSource>(this.ScenePrefabProperty.PrefabId);
            foreach (var item in source.SceneData.Items)
            {
                var child = item.Entity;
                this.PrepareEntity(child);
                this.Owner.AddChild(child);
            }

            Trace.WriteLine($"Prefab with id '{this.ScenePrefabProperty.PrefabId}' created in {st.ElapsedMilliseconds}ms");
        }

        private void PrepareEntity(Entity entity)
        {
            entity.Id = Guid.NewGuid();
            entity.Flags = HideFlags.DontSave | HideFlags.DontShow;
            foreach (var component in entity.Components)
            {
                component.Id = Guid.NewGuid();

                if (this.DuplicateMaterials &&
                    component is MaterialComponent materialComponent)
                {
                    materialComponent.Material = materialComponent.Material?.Clone();
                }
            }

            foreach (var child in entity.ChildEntities.ToArray())
            {
                entity.DetachChild(child);
                this.PrepareEntity(child);
                entity.AddChild(child);
            }
        }
    }
}
