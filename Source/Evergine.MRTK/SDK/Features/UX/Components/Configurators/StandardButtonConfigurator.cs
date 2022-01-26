﻿// Copyright © Evergine S.L. All rights reserved. Use is subject to license terms.
using Evergine.Common.Attributes;
using Evergine.Common.Graphics;
using Evergine.Components.Graphics3D;
using Evergine.Framework;
using Evergine.Framework.Graphics;
using Evergine.MRTK.Toolkit.GUI;

namespace Evergine.MRTK.SDK.Features.UX.Components.Configurators
{
    /// <summary>
    /// Configures some UI elements for a standard UI button.
    /// </summary>
    public class StandardButtonConfigurator : Component
    {
        private readonly MaterialConfigurator plateConfigurator;
        private readonly MaterialConfigurator iconConfigurator;

        [BindComponent(source: BindComponentSource.ChildrenSkipOwner, tag: "PART_Plate", isRequired: false)]
        private MaterialComponent plateMaterial = null;

        [BindComponent(source: BindComponentSource.ChildrenSkipOwner, tag: "PART_Icon", isRequired: false)]
        private MaterialComponent iconMaterial = null;

        [BindComponent(source: BindComponentSource.ChildrenSkipOwner, tag: "PART_Text", isRequired: false)]
        private Text3D buttonText = null;

        private Material plate;
        private Material icon;

        private Color primaryColor = Color.White;
        private string text;
        private FontFamilySourceProperty fontFamilySourceProperty;

        /// <summary>
        /// Initializes a new instance of the <see cref="StandardButtonConfigurator"/> class.
        /// </summary>
        public StandardButtonConfigurator()
        {
            this.plateConfigurator = new MaterialConfigurator();
            this.iconConfigurator = new MaterialConfigurator();
        }

        /// <summary>
        /// Gets or sets plate material.
        /// </summary>
        public Material Plate
        {
            get => this.plate;

            set
            {
                if (this.plate != value)
                {
                    this.plate = value;
                    this.plateConfigurator.Material = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether back plate material should be a new material instance.
        /// </summary>
        public bool CreatesNewBackPlateMaterialInstance { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether icon material should be a new material instance.
        /// </summary>
        public bool CreatesNewIconMaterialInstance { get; set; }

        /// <summary>
        /// Gets or sets button icon.
        /// </summary>
        public Material Icon
        {
            get => this.icon;

            set
            {
                if (this.icon != value)
                {
                    this.icon = value;
                    this.iconConfigurator.Material = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets button primary color. This color is used to tint icon and set
        /// text color.
        /// </summary>
        public Color PrimaryColor
        {
            get => this.primaryColor;
            set
            {
                if (this.primaryColor != value)
                {
                    this.primaryColor = value;
                    this.OnPrimaryColorUpdate();
                }
            }
        }

        /// <summary>
        /// Gets or sets button text.
        /// </summary>
        public string Text
        {
            get => this.text;

            set
            {
                if (this.text != value)
                {
                    this.text = value;
                    this.OnUpdateText();
                }
            }
        }

        /// <summary>
        /// Gets or sets the text font family source to use.
        /// </summary>
        [RenderProperty(CustomPropertyName = "FontFamily", Tooltip = "The font family source to use in the button text.")]
        public FontFamilySourceProperty FontFamilySourceProperty
        {
            get
            {
                if (this.fontFamilySourceProperty == null)
                {
                    this.fontFamilySourceProperty = new FontFamilySourceProperty();
                    this.fontFamilySourceProperty.OnFontFamilySourceChanged += this.FontFamilySourceProperty_OnFontFamilySourceChanged;
                }

                return this.fontFamilySourceProperty;
            }

            set
            {
                if (this.fontFamilySourceProperty != value)
                {
                    if (this.fontFamilySourceProperty != null)
                    {
                        this.fontFamilySourceProperty.OnFontFamilySourceChanged -= this.FontFamilySourceProperty_OnFontFamilySourceChanged;
                    }

                    this.fontFamilySourceProperty = value;

                    if (this.fontFamilySourceProperty != null)
                    {
                        this.fontFamilySourceProperty.OnFontFamilySourceChanged += this.FontFamilySourceProperty_OnFontFamilySourceChanged;
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

            this.plateConfigurator.UseAssetManager(this.Managers.AssetSceneManager);
            this.iconConfigurator.UseAssetManager(this.Managers.AssetSceneManager);

            this.plateConfigurator.CreatesNewMaterialInstance = this.CreatesNewBackPlateMaterialInstance;
            this.iconConfigurator.CreatesNewMaterialInstance = this.CreatesNewIconMaterialInstance;

            this.plateConfigurator.TargetMaterialComponent = this.plateMaterial;
            this.iconConfigurator.TargetMaterialComponent = this.iconMaterial;

            return true;
        }

        /// <inheritdoc />
        protected override void OnActivated()
        {
            base.OnActivated();
            this.plateConfigurator.Apply();
            this.iconConfigurator.Apply();
            this.OnUpdateText();
            this.OnUpdateTextColor();
            this.OnUpdateFontFamilySource();
        }

        private void OnPrimaryColorUpdate()
        {
            this.iconConfigurator.TintColor = this.primaryColor;
            this.OnUpdateTextColor();
        }

        private void OnUpdateText()
        {
            if (this.buttonText != null)
            {
                this.buttonText.Text = this.text;
            }
        }

        private void OnUpdateTextColor()
        {
            if (this.buttonText != null)
            {
                this.buttonText.Foreground = this.primaryColor;
            }
        }

        private void OnUpdateFontFamilySource()
        {
            if (this.buttonText != null)
            {
                this.buttonText.FontFamilySourceProperty.FontFamilySource = this.FontFamilySourceProperty.FontFamilySource;
            }
        }

        private void FontFamilySourceProperty_OnFontFamilySourceChanged(object sender, System.EventArgs e)
        {
            this.OnUpdateFontFamilySource();
        }
    }
}
